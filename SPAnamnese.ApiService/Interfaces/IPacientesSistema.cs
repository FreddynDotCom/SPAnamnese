using SPAnamnese.ApiService.DTOs;

namespace SPAnamnese.ApiService.Interfaces
{
    public class PacienteSistemaFiltroDTO
    {
        public int ID { get; set; }
        public string? NOME { get; set; } = null;
        public string? CPF { get; set; } = null;
        public DateTime DATANASCIMENTO { get; set; }
        public string? TELEFONE { get; set; } = null;
        public string? EMAIL { get; set; } = null;
        public string? ENDERECO { get; set; } = null;
        public string? RESPONSAVELLEGAL { get; set; } = null;
        public string? SEXO { get; set; } = null;
    }

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
        Task<PacienteSistemaDTO> UpdatePacienteAsync(int id, PacienteSistemaFiltroDTO pacienteDTO);

        ///<summary>
        /// [Pacientes] - Paged
        ///</summary>
        /// <returns>Pega uma lista de PACIENTES, e pagina ela</returns>
        Task<(List<PacienteSistemaDTO> items, int totalCount)> PagedPacientesAsync(int pageSize, int pageNumber, PacienteSistemaFiltroDTO? pacienteParams);
    }
}
