using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPAnamnese.ApiService.Data;
using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;
using SPAnamnese.ApiService.Models;

namespace SPAnamnese.ApiService.Services
{
    public class AnamneseService : IAnamnese
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public AnamneseService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        ///<summary>
        /// [Anamnese] - Create
        ///</summary>
        /// <returns>Cria e retorna Anamnese</returns>
        public async Task<AnamneseDTO> CreateAnamneseAsync(AnamneseDTO anamneseDTO)
        {
            //if (anamneseDTO.ProfissionalId == null)
            //    throw new ArgumentException("Sem profissional.");

            if (anamneseDTO.PacienteId == 0)
                throw new ArgumentException("Sem paciente.");

            var anamneseCriada = await _db.AddAsync(_mapper.Map<tbanamnese>(anamneseDTO));
            await _db.SaveChangesAsync();

            return _mapper.Map<AnamneseDTO>(anamneseCriada.Entity);
        }

        ///<summary>
        /// [Anamnese] - Delete
        ///</summary>
        /// <returns>Atualiza um registro de Anamnese</returns>
        public Task<AnamneseDTO> DeleteAnamneseAsync(int id)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// [Anamnese] - Get By Id
        ///</summary>
        /// <returns>Retorna Anamnese de ID [X].</returns>
        public Task<AnamneseDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// [Anamnese] - Update
        ///</summary>
        /// <returns>Atualiza um registro de Anamnese</returns>
        public async Task<AnamneseDTO> UpdateAnamneseAsync(int id, AnamneseDTO anamneseDTO)
        {
            var anamneseAtualizada = _db.tbanamneses.FirstOrDefault(a => a.Id == id);
            if (anamneseAtualizada == null) throw new ArgumentException("Registro não encontrado.");

            _mapper.Map(anamneseDTO, anamneseAtualizada);
            await _db.SaveChangesAsync();
            return _mapper.Map<AnamneseDTO>(anamneseAtualizada);
        }

        ///<summary>
        /// [Anamnese] - Visualizar
        ///</summary>
        /// <returns>Retorna um registro de Anamnese</returns>
        public async Task<AnamneseDTO> VisualizarAnamneseAsync(int id)
        {
            var anamneseVisualizada = await _db.tbanamneses
                .Include(i => i.Paciente)
                //.Include(i => i.Profissional)
                .FirstOrDefaultAsync(a => a.PacienteId == id);
            if (anamneseVisualizada == null) throw new ArgumentException("Registro não encontrado.");

            return _mapper.Map<AnamneseDTO>(anamneseVisualizada);
        }
    }
}
