using SPAnamnese.Client.Services;
using System.Net;
using System.Net.Http.Headers;

namespace SPAnamnese.Client.Auth
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

            // Guarda uma cópia da requisição original, pois um HttpRequestMessage
            // só pode ser enviado uma vez - se precisarmos reenviar após renovar
            // o token, precisamos de uma nova instância.
            var requisicaoOriginalClonada = await ClonarRequisicaoAsync(request);

            var resposta = await base.SendAsync(request, cancellationToken);

            if (resposta.StatusCode != HttpStatusCode.Unauthorized)
            {
                return resposta;
            }

            // Token expirado ou inválido: tenta renovar uma única vez.
            var novoAccessToken = await _authService.ObterAccessTokenAsync();
            if (novoAccessToken is null)
            {
                // Renovação falhou (refresh token também expirou/foi revogado).
                // Devolve a resposta 401 original; a camada de UI (TokenRenewalService
                // ou o próprio componente) é responsável por redirecionar para /login.
                return resposta;
            }

            requisicaoOriginalClonada.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", novoAccessToken);

            return await base.SendAsync(requisicaoOriginalClonada, cancellationToken);
        }

        private static async Task<HttpRequestMessage> ClonarRequisicaoAsync(HttpRequestMessage original)
        {
            var clone = new HttpRequestMessage(original.Method, original.RequestUri)
            {
                Version = original.Version
            };

            if (original.Content is not null)
            {
                var bytes = await original.Content.ReadAsByteArrayAsync();
                var novoConteudo = new ByteArrayContent(bytes);
                foreach (var header in original.Content.Headers)
                {
                    novoConteudo.Headers.Add(header.Key, header.Value);
                }
                clone.Content = novoConteudo;
            }

            foreach (var header in original.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var opcao in original.Options)
            {
                clone.Options.Set(new HttpRequestOptionsKey<object?>(opcao.Key), opcao.Value);
            }

            return clone;
        }
    }

}
