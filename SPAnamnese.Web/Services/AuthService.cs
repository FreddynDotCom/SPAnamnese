using Blazored.LocalStorage;
using SPAnamnese.Web.Auth;
using SPAnamnese.Web.Models;

namespace SPAnamnese.Web.Services
{
    public class AuthService
    {
        private const string ChaveAccessToken = "accessToken";
        private const string ChaveExpiracaoAccessToken = "accessTokenExpiraEm";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;
        private readonly JwtAuthenticationStateProvider _authStateProvider;

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
        /// Usado para login/registro, evitando recursão.
        /// </summary>
        private HttpClient ClienteAnonimo => _httpClientFactory.CreateClient("API.Anonimo");

        public async Task<(bool Sucesso, string? Erro)> LoginAsync(string email, string senha)
        {
            var resposta = await ClienteAnonimo.PostAsJsonAsync(
                "auth/login",
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
                "auth/registrar",
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
                tokens.AccessTokenExpiraEm.ToString("o"));
        }
    }
}