using SPAnamnese.Client.DTOs;
using System.Net.Http.Json;

namespace SPAnamnese.Client.ServiceWeb
{
    public class AnamneseServiceWeb
    {
        private readonly HttpClient _httpClient;

        public AnamneseServiceWeb(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AnamneseDTO> VisualizarAnamnese(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Anamnese/VisualizarById/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<AnamneseDTO>() ?? new AnamneseDTO();
            }
            catch
            {
                return new AnamneseDTO();
            }
        }

        public async Task<AnamneseDTO> UpdateAnamneseAsync(int id, AnamneseDTO anamneseDTO)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Anamnese/UpdateAnamnese/{id}", anamneseDTO);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<AnamneseDTO>() ?? new AnamneseDTO();
            }
            catch
            {
                return new AnamneseDTO();
            }
        }

        public async Task<AnamneseDTO> CreateAnamneseAsync(AnamneseDTO anamneseDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Anamnese/CreateAnamnese", anamneseDTO);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMsg);
            }

            return await response.Content.ReadFromJsonAsync<AnamneseDTO>() ?? new AnamneseDTO();
        }
    }
}
