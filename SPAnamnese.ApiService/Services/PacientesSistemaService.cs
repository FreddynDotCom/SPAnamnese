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
        public async Task<PacienteSistemaDTO> UpdatePacienteAsync(int id, PacienteSistemaFiltroDTO pacienteDTO)
        {
            var pacienteUpdate = await _db.tbpacientes.FirstOrDefaultAsync(p => p.ID == id);
            if (pacienteUpdate == null)
                throw new ArgumentException("Registro não encontrado.");

            if (!string.IsNullOrWhiteSpace(pacienteDTO.CPF))
            {
                if (!LimpezaDados.LimparEValidarCpf(pacienteDTO.CPF, out string cpfLimpo))
                    throw new ArgumentException("CPF inválido.");
                pacienteUpdate.CPF = cpfLimpo;
            }

            if (!string.IsNullOrWhiteSpace(pacienteDTO.EMAIL))
            {
                if (!LimpezaDados.LimparEValidarEmail(pacienteDTO.EMAIL, out string emailLimpo))
                    throw new ArgumentException("E-mail inválido.");
                pacienteUpdate.EMAIL = emailLimpo;
            }

            if (!string.IsNullOrWhiteSpace(pacienteDTO.TELEFONE))
            {
                if (!LimpezaDados.LimparEValidarTelefone(pacienteDTO.TELEFONE, out string telefoneLimpo))
                    throw new ArgumentException("Telefone inválido.");
                pacienteUpdate.TELEFONE = telefoneLimpo;
            }

            if (!string.IsNullOrWhiteSpace(pacienteDTO.NOME))
                pacienteUpdate.NOME = pacienteDTO.NOME;

            if (!string.IsNullOrWhiteSpace(pacienteDTO.SEXO))
                pacienteUpdate.SEXO = pacienteDTO.SEXO;

            if (!string.IsNullOrWhiteSpace(pacienteDTO.RESPONSAVELLEGAL))
                pacienteUpdate.RESPONSAVELLEGAL = pacienteDTO.RESPONSAVELLEGAL;

            if (!string.IsNullOrWhiteSpace(pacienteDTO.ENDERECO))
                pacienteUpdate.ENDERECO = pacienteDTO.ENDERECO;

            if (pacienteDTO.DATANASCIMENTO != default)
                pacienteUpdate.DATANASCIMENTO = pacienteDTO.DATANASCIMENTO;

            await _db.SaveChangesAsync();

            return _mapper.Map<PacienteSistemaDTO>(pacienteUpdate);
        }

        //TODO: Fazer DELETE depois, tem que adicionar STATUS na tabela PACIENTES.

        ///<summary>
        /// [Pacientes] - Paged
        ///</summary>
        /// <returns>Pega uma lista de PACIENTES, e pagina ela</returns>
        public async Task<(List<PacienteSistemaDTO> items, int totalCount)> PagedPacientesAsync(int pageSize, int pageNumber, PacienteSistemaFiltroDTO? pacienteParams)
        {
            if (pageSize < 1) pageSize = 1;
            if (pageNumber < 1) pageNumber = 1;

            var listaPacientes = _db.tbpacientes.AsQueryable();

            //Limpa e busca filtros
            if (pacienteParams != null)
            {
                if (!string.IsNullOrEmpty(pacienteParams.CPF))
                {
                    var cpfLimpo = LimpezaDados.LimparCpfCnpj(pacienteParams.CPF);
                    listaPacientes = listaPacientes.Where(p => p.CPF.Contains(cpfLimpo));
                }

                if (!string.IsNullOrEmpty(pacienteParams.EMAIL))
                {
                    var emailLimpo = LimpezaDados.LimparEmail(pacienteParams.EMAIL);
                    listaPacientes = listaPacientes.Where(p => p.EMAIL.Contains(emailLimpo));
                }

                if (!string.IsNullOrEmpty(pacienteParams.TELEFONE))
                {
                    var telefoneLimpo = LimpezaDados.LimparTelefone(pacienteParams.TELEFONE);
                    listaPacientes = listaPacientes.Where(p => p.TELEFONE.Contains(telefoneLimpo));
                }

                if (!string.IsNullOrEmpty(pacienteParams.NOME))
                    listaPacientes = listaPacientes.Where(p => p.NOME.Contains(pacienteParams.NOME));
            }

            var totalCount = await listaPacientes.CountAsync();
            var items = await listaPacientes
                .OrderBy(p => p.NOME)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pacienteRetorno = _mapper.Map<List<PacienteSistemaDTO>>(items);
            //Verifica se tem registro na tabela de Anamnese
            foreach (var paciente in pacienteRetorno)
            {
                var temAnamnese = await _db.tbanamneses.AnyAsync(a => a.PacienteId == paciente.ID);
                paciente.TEMANAMNESE = temAnamnese;
            }

            return (pacienteRetorno, totalCount);
        }
    }
}
