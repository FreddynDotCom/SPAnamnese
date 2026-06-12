using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPAnamnese.ApiService.Data;
using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;
using SPAnamnese.ApiService.Models;
using SPAnamnese.ApiService.Utils;

namespace SPAnamnese.ApiService.Services
{
    public class PacientesSistemaService : IPacientesSistema
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public PacientesSistemaService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        ///<summary>
        /// [Pacientes] - Get By Id
        ///</summary>
        /// <returns>Retorna PACIENTE de ID [X].</returns>
        public async Task<PacienteSistemaDTO> GetByIdAsync(int id)
        {
            var paciente = await _db.tbpacientes.FirstOrDefaultAsync(p => p.ID == id);
            if (paciente == null)
            {
                throw new ArgumentException("Registro não encontrado.");
            }

            return _mapper.Map<PacienteSistemaDTO>(paciente);
        }

        ///<summary>
        /// [Pacientes] - Create
        ///</summary>
        /// <returns>Cria e retorna PACIENTE</returns>
        public async Task<PacienteSistemaDTO> CreatePacienteAsync(PacienteSistemaDTO pacienteDTO)
        {
            //Limpa e valida tudo de uma vez
            if (!LimpezaDados.LimparEValidarCpf(pacienteDTO.CPF, out string cpfLimpo))
                throw new ArgumentException("CPF inválido.");

            if (!LimpezaDados.LimparEValidarEmail(pacienteDTO.EMAIL, out string emailLimpo))
                throw new ArgumentException("E-mail inválido.");

            if (!LimpezaDados.LimparEValidarTelefone(pacienteDTO.TELEFONE, out string telefoneLimpo))
                throw new ArgumentException("Telefone inválido.");

            //Substitui os valores já limpos no DTO
            pacienteDTO.CPF = cpfLimpo;
            pacienteDTO.EMAIL = emailLimpo;
            pacienteDTO.TELEFONE = telefoneLimpo;

            var pacienteCriado = await _db.AddAsync(_mapper.Map<tbpaciente>(pacienteDTO));
            await _db.SaveChangesAsync();

            return _mapper.Map<PacienteSistemaDTO>(pacienteCriado.Entity);
        }

        ///<summary>
        /// [Pacientes] - Update
        ///</summary>
        /// <returns>Atualiza um registro de PACIENTE</returns>
        public async Task<PacienteSistemaDTO> UpdatePacienteAsync(int id, PacienteSistemaDTO pacienteDTO)
        {
            //Busca paciente via ID
            var pacienteUpdate = await _db.tbpacientes.FirstOrDefaultAsync(p => p.ID == id);
            if (pacienteUpdate == null)
            {
                throw new ArgumentException("Registro não encontrado.");
            }

            //Limpa e valida tudo de uma vez
            if (!LimpezaDados.LimparEValidarCpf(pacienteDTO.CPF, out string cpfLimpo))
                throw new ArgumentException("CPF inválido.");

            if (!LimpezaDados.LimparEValidarEmail(pacienteDTO.EMAIL, out string emailLimpo))
                throw new ArgumentException("E-mail inválido.");

            if (!LimpezaDados.LimparEValidarTelefone(pacienteDTO.TELEFONE, out string telefoneLimpo))
                throw new ArgumentException("Telefone inválido.");

            //Substitui os valores já limpos no DTO
            pacienteDTO.ID = pacienteUpdate.ID;
            _mapper.Map(pacienteDTO, pacienteUpdate);
            pacienteUpdate.CPF = cpfLimpo;
            pacienteUpdate.EMAIL = emailLimpo;
            pacienteUpdate.TELEFONE = telefoneLimpo;

            await _db.SaveChangesAsync();

            return _mapper.Map<PacienteSistemaDTO>(pacienteUpdate);
        }

        //TODO: Fazer DELETE depois, tem que adicionar STATUS na tabela PACIENTES.
    }
}
