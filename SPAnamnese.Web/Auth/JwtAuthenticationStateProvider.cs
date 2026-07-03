using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SPAnamnese.Web.Auth
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private static readonly AuthenticationState EstadoAnonimo =
            new(new ClaimsPrincipal(new ClaimsIdentity()));

        public JwtAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? accessToken;
            try
            {
                accessToken = await _localStorage.GetItemAsStringAsync("accessToken");
            }
            catch
            {
                // Pode falhar em pré-renderização ou se o storage ainda não estiver disponível.
                return EstadoAnonimo;
            }

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return EstadoAnonimo;
            }

            // O LocalStorage guarda strings com aspas JSON; remove se necessário.
            accessToken = accessToken.Trim('"');

            try
            {
                var claims = JwtClaimsParser.ParseClaimsFromJwt(accessToken);
                var identity = new ClaimsIdentity(claims, authenticationType: "jwt");
                var principal = new ClaimsPrincipal(identity);
                return new AuthenticationState(principal);
            }
            catch
            {
                // Token corrompido/ilegível: trata como não autenticado.
                return EstadoAnonimo;
            }
        }

        /// <summary>Notifica o Blazor de que um novo login foi realizado.</summary>
        public void NotificarLogin(string accessToken)
        {
            var claims = JwtClaimsParser.ParseClaimsFromJwt(accessToken);
            var identity = new ClaimsIdentity(claims, authenticationType: "jwt");
            var principal = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        /// <summary>Notifica o Blazor de que o usuário saiu (logout ou falha na renovação).</summary>
        public void NotificarLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(EstadoAnonimo));
        }
    }
}
