using Microsoft.EntityFrameworkCore;
using SPAnamnese.ApiService.Data;
using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;
using System.Runtime.CompilerServices;

namespace SPAnamnese.ApiService.Services
{
    public class TesteService : ITeste
    {
        private readonly AppDbContext _db;

        public TesteService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PacienteSistemaDTO> GetById(int id)
        {
            var paciente = await _db.tbpacientes.FirstOrDefaultAsync(p => p.ID == id);

            PacienteSistemaDTO pacienteRetorno = new()
            {
                ID = paciente.ID,
                NOME = paciente.NOME,
            };

            return pacienteRetorno;
        }
    }
}
