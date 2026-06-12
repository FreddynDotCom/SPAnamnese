using SPAnamnese.ApiService.DTOs;

namespace SPAnamnese.ApiService.Interfaces
{
    public interface IPacientesSistema
    {
        ///<summary>
        /// [Pacientes] - Get By Id
        ///</summary>
        /// <returns>Retorna PACIENTE de ID [X].</returns>
        Task<PacienteSistemaDTO> GetByIdAsync(int id);

        ///<summary>
        /// [Pacientes] - Create
        ///</summary>
        /// <returns>Cria e retorna PACIENTE</returns>
        Task<PacienteSistemaDTO> CreatePacienteAsync(PacienteSistemaDTO pacienteDTO);

        ///<summary>
        /// [Pacientes] - Update
        ///</summary>
        /// <returns>Atualiza um registro de PACIENTE</returns>
        Task<PacienteSistemaDTO> UpdatePacienteAsync(int id, PacienteSistemaDTO pacienteDTO);
    }
}
