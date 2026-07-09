using System.Net.Http.Json;
using Blazored.LocalStorage;
using SPAnamnese.Client.Auth;
using SPAnamnese.Client.Models;

namespace SPAnamnese.Client.Services;

/// <summary>
/// Serviço central responsável por login, registro, logout e renovação
/// (refresh) do token JWT. Mantém os tokens persistidos no localStorage.
/// </summary>
public class AuthService
{
    private const string ChaveAccessToken = "accessToken";
    private const string ChaveExpiracaoAccessToken = "accessTokenExpiraEm";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    private readonly JwtAuthenticationStateProvider _authStateProvider;

    // Evita que múltiplas requisições simultâneas disparem várias
    // renovações de token ao mesmo tempo (condição de corrida).
    private readonly SemaphoreSlim _refreshLock = new(1, 1);

    public AuthService(
        IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorage,
        JwtAuthenticationStateProvider authStateProvider)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    /// <summary>
    /// Cliente HTTP "anônimo", sem o handler que injeta o Bearer token.
    /// Usado para login/registro/refresh, evitando recursão (o handler
    /// também reagiria a um eventual 401 nessas próprias chamadas).
    /// </summary>
    private HttpClient ClienteAnonimo => _httpClientFactory.CreateClient("API.Anonimo");

    public async Task<(bool Sucesso, string? Erro)> LoginAsync(string email, string senha)
    {
        var resposta = await ClienteAnonimo.PostAsJsonAsync(
            "api/auth/login",
            new LoginRequest { Email = email, Senha = senha },
            JsonDefaults.Options);

        if (!resposta.IsSuccessStatusCode)
        {
            return (false, "E-mail ou senha inválidos.");
        }

        var tokens = await resposta.Content.ReadFromJsonAsync<TokenResponse>(JsonDefaults.Options);
        if (tokens is null)
        {
            return (false, "Resposta inesperada do servidor.");
        }

        await ArmazenarTokensAsync(tokens);
        _authStateProvider.NotificarLogin(tokens.AccessToken);
        return (true, null);
    }

    public async Task<(bool Sucesso, string? Erro)> RegistrarAsync(
        string nomeCompleto, string email, string senha, string role)
    {
        var resposta = await ClienteAnonimo.PostAsJsonAsync(
            "api/auth/registrar",
            new RegisterRequest { NomeCompleto = nomeCompleto, Email = email, Senha = senha, Role = role },
            JsonDefaults.Options);

        if (resposta.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var corpoErro = await resposta.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(corpoErro)
            ? "Não foi possível concluir o cadastro."
            : corpoErro);
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(ChaveAccessToken);
        await _localStorage.RemoveItemAsync(ChaveExpiracaoAccessToken);
        _authStateProvider.NotificarLogout();
    }

    public async Task<string?> ObterAccessTokenAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync(ChaveAccessToken);
        return token?.Trim('"');
    }

    public async Task<DateTime?> ObterAccessTokenExpiracaoAsync()
    {
        var texto = await _localStorage.GetItemAsStringAsync(ChaveExpiracaoAccessToken);
        texto = texto?.Trim('"');

        if (string.IsNullOrWhiteSpace(texto))
        {
            return null;
        }

        return DateTime.TryParse(
            texto,
            null,
            System.Globalization.DateTimeStyles.RoundtripKind,
            out var data)
            ? data
            : null;
    }

    private async Task ArmazenarTokensAsync(TokenResponse tokens)
    {
        await _localStorage.SetItemAsStringAsync(ChaveAccessToken, tokens.AccessToken);
        await _localStorage.SetItemAsStringAsync(
            ChaveExpiracaoAccessToken,
            tokens.AccessTokenExpiraEm.ToString("o")); // formato ISO 8601 ("round-trip")
    }
}
