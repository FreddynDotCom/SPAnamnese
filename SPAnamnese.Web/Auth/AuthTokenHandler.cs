using SPAnamnese.Web.Services;
using System.Net.Http.Headers;

namespace SPAnamnese.Web.Auth
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly AuthService _authService;

        public AuthTokenHandler(AuthService authService)
        {
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _authService.ObterAccessTokenAsync();
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            // Sem refresh token: se der 401, a UI (PaginaProtegida) trata o redirecionamento pro /login.
            return await base.SendAsync(request, cancellationToken);
        }
    }
}