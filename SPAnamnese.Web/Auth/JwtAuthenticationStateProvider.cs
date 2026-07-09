using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SPAnamnese.Web.Services;

namespace SPAnamnese.Web.Auth
{
    /// <summary>
    /// Provedor de estado de autenticação baseado no JWT salvo no LocalStorage.
    /// O token também fica em cache no TokenStore (escopo do circuito) para
    /// evitar chamadas repetidas de JS interop e alimentar o
    /// AuthorizationMessageHandlerScoped nas requisições HTTP.
    /// </summary>
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const string ChaveToken = "accessToken";

        private readonly ILocalStorageService _localStorage;
        private readonly TokenStore _tokenStore;
        private static readonly AuthenticationState EstadoAnonimo =
            new(new ClaimsPrincipal(new ClaimsIdentity()));

        public JwtAuthenticationStateProvider(ILocalStorageService localStorage, TokenStore tokenStore)
        {
            _localStorage = localStorage;
            _tokenStore = tokenStore;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!string.IsNullOrWhiteSpace(_tokenStore.Token))
            {
                return MontarEstado(_tokenStore.Token);
            }

            string? token;
            try
            {
                token = await _localStorage.GetItemAsStringAsync(ChaveToken);
            }
            catch
            {
                // Prerender: ainda não há JS interop disponível.
                return EstadoAnonimo;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return EstadoAnonimo;
            }

            token = token.Trim('"');
            _tokenStore.Token = token;
            return MontarEstado(token);
        }

        public async Task NotificarLoginAsync(string accessToken)
        {
            _tokenStore.Token = accessToken;
            await _localStorage.SetItemAsStringAsync(ChaveToken, accessToken);
            NotifyAuthenticationStateChanged(Task.FromResult(MontarEstado(accessToken)));
        }

        public async Task NotificarLogoutAsync()
        {
            _tokenStore.Token = null;
            await _localStorage.RemoveItemAsync(ChaveToken);
            NotifyAuthenticationStateChanged(Task.FromResult(EstadoAnonimo));
        }

        /// <summary>
        /// Recarrega o estado a partir do LocalStorage após a primeira
        /// renderização interativa (chamar em OnAfterRenderAsync(firstRender)
        /// do MainLayout).
        /// </summary>
        public async Task RecarregarAposConexaoAsync()
        {
            var estado = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(estado));
        }

        private static AuthenticationState MontarEstado(string token)
        {
            var claims = JwtClaimsParser.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, authenticationType: "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}