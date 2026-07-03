using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;
using System.Net.Http.Json;

namespace SPAnamnese.Web.ServiceWeb
{
    public class PacientesSistemaServiceWeb
    {
        private readonly HttpClient _httpClient;

        public PacientesSistemaServiceWeb(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PacienteSistemaDTO> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"PacientesSistema/GetPacienteById/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<PacienteSistemaDTO>() ?? new PacienteSistemaDTO();
            }
            catch
            {
                return new PacienteSistemaDTO();
            }
        }

        public async Task<PacienteSistemaDTO> CreatePacienteAsync(PacienteSistemaDTO pacienteDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("PacientesSistema/CreatePaciente", pacienteDTO);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMsg);
            }

            return await response.Content.ReadFromJsonAsync<PacienteSistemaDTO>() ?? new PacienteSistemaDTO();
        }

        public async Task<PacienteSistemaDTO> UpdatePacienteAsync(int id, PacienteSistemaFiltroDTO pacienteDTO)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"PacientesSistema/UpdatePaciente/{id}", pacienteDTO);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<PacienteSistemaDTO>() ?? new PacienteSistemaDTO();
            }
            catch
            {
                return new PacienteSistemaDTO();
            }
        }

        public async Task<(List<PacienteSistemaDTO> Items, int TotalCount)> PagedPacientesAsync(
            PacienteSistemaFiltroDTO? filtro = null,
            int pageSize = 10,
            int pageNumber = 1)
        {
            var url = $"PacientesSistema/PagedPacientes?pageSize={pageSize}&pageNumber={pageNumber}";
            var response = await _httpClient.PutAsJsonAsync(url, filtro);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PagedResult<PacienteSistemaDTO>>();
            return (result?.Items ?? new List<PacienteSistemaDTO>(), result?.TotalCount ?? 0);
        }
    }

    file class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
    }
}