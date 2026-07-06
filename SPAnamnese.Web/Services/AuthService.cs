using SPAnamnese.ApiService.DTOs;
using System.Net.Http.Json;

namespace SPAnamnese.Web.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> Login(string email, string senha)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", new { email, senha });
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var resultado = await response.Content.ReadFromJsonAsync<TokenResponseDTO>();
            return resultado?.AccessToken;
        }
    }
}