using SPAnamnese.Client.Services;
using Microsoft.AspNetCore.Components;

namespace SPAnamnese.Client.Auth;

/// <summary>
/// Serviço que verifica periodicamente (em segundo plano) se o access token
/// está próximo de expirar e, nesse caso, dispara a renovação automaticamente
/// - sem esperar que uma chamada de API falhe com 401.
/// </summary>
/// <remarks>
/// Se a renovação falhar (refresh token expirado/revogado), redireciona
/// automaticamente o usuário para a tela de login, conforme exigido pelo projeto.
/// Deve ser iniciado uma única vez, normalmente em <c>MainLayout.OnInitialized</c>.
/// </remarks>
public class TokenRenewalService : IDisposable
{
    // Margem de segurança: renova o token um pouco antes dele expirar de fato.
    private static readonly TimeSpan MargemRenovacao = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan IntervaloVerificacao = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan AtrasoInicial = TimeSpan.FromSeconds(10);

    private readonly AuthService _authService;
    private readonly NavigationManager _navigationManager;
    private Timer? _timer;
    private bool _verificando;

    public TokenRenewalService(AuthService authService, NavigationManager navigationManager)
    {
        _authService = authService;
        _navigationManager = navigationManager;
    }

    /// <summary>Inicia o monitoramento periódico. Chamadas repetidas são ignoradas.</summary>
    public void Iniciar()
    {
        if (_timer is not null)
        {
            return;
        }

        _timer = new Timer(
            callback: async _ => await VerificarENecessarioRenovarAsync(),
            state: null,
            dueTime: AtrasoInicial,
            period: IntervaloVerificacao);
    }

    private async Task VerificarENecessarioRenovarAsync()
    {
        // Evita execuções concorrentes do próprio timer.
        if (_verificando)
        {
            return;
        }
        _verificando = true;

        try
        {
            var accessToken = await _authService.ObterAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                // Usuário não está logado; nada a fazer.
                return;
            }

            var expiracao = await _authService.ObterAccessTokenExpiracaoAsync();
            if (expiracao is null)
            {
                return;
            }

            var faltaParaExpirar = expiracao.Value - DateTime.UtcNow;
            if (faltaParaExpirar > MargemRenovacao)
            {
                // Ainda não está perto de expirar - nada a fazer por agora.
                return;
            }

            var novoToken = await _authService.ObterAccessTokenAsync();
            if (novoToken is null)
            {
                // Falha na renovação automática: redireciona para o login,
                // conforme exigido pelo comportamento do projeto.
                _navigationManager.NavigateTo("/login?expirado=1", forceLoad: false);
            }
        }
        catch
        {
            // Falhas de rede temporárias não devem derrubar o timer;
            // a próxima verificação tentará novamente.
        }
        finally
        {
            _verificando = false;
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
