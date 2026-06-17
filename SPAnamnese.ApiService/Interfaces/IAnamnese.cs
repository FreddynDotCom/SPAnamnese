using SPAnamnese.ApiService.DTOs;

namespace SPAnamnese.ApiService.Interfaces
{
    public interface IAnamnese
    {
        ///<summary>
        /// [Anamnese] - Get By Id
        ///</summary>
        /// <returns>Retorna Anamnese de ID [X].</returns>
        Task<AnamneseDTO> GetByIdAsync(int id);

        ///<summary>
        /// [Anamnese] - Create
        ///</summary>
        /// <returns>Cria e retorna Anamnese</returns>
        Task<AnamneseDTO> CreateAnamneseAsync(AnamneseDTO anamneseDTO);

        ///<summary>
        /// [Anamnese] - Update
        ///</summary>
        /// <returns>Atualiza um registro de Anamnese</returns>
        Task<AnamneseDTO> UpdateAnamneseAsync(int id, AnamneseDTO anamneseDTO);

        ///<summary>
        /// [Anamnese] - Delete
        ///</summary>
        /// <returns>Atualiza um registro de Anamnese</returns>
        Task<AnamneseDTO> DeleteAnamneseAsync(int id);

        ///<summary>
        /// [Anamnese] - Visualizar
        ///</summary>
        /// <returns>Retorna um registro de Anamnese</returns>
        Task<AnamneseDTO> VisualizarAnamneseAsync(int id);
    }
}
