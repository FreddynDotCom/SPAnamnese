using SPAnamnese.ApiService.DTOs;

namespace SPAnamnese.ApiService.Interfaces
{
    public interface ITeste
    {
        Task<PacienteSistemaDTO> GetById(int id);
    }
}
