using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;

namespace SPAnamnese.ApiService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesSistemaController : ControllerBase
    {
        private readonly IPacientesSistema _service;
        private readonly IMapper _mapper;

        public PacientesSistemaController(IPacientesSistema service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        ///<summary>
        /// [Pacientes] - Get By Id
        ///</summary>
        /// <returns>Retorna PACIENTE de ID [X].</returns>
        [HttpGet("GetPacienteById/{id}")]
        public async Task<IActionResult> GetPacienteById(int id)
        {
            try
            {
                var pacienteObtido = await _service.GetByIdAsync(id);
                return Ok(pacienteObtido);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro interno no servidor: {ex.Message}");
            }
        }

        ///<summary>
        /// [Pacientes] - Create
        ///</summary>
        /// <returns>Cria e retorna PACIENTE</returns>
        [HttpPost("CreatePaciente")]
        public async Task<IActionResult> CreatePaciente([FromBody] PacienteSistemaDTO pacienteDTO)
        {
            try
            {
                var paciente = await _service.CreatePacienteAsync(pacienteDTO);
                return Ok(paciente);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro interno no servidor: {ex.Message}");
            }
        }

        ///<summary>
        /// [Pacientes] - Update
        ///</summary>
        /// <returns>Atualiza um registro de PACIENTE</returns>
        [HttpPut("UpdatePaciente/{id}")]
        public async Task<IActionResult> UpdatePaciente(int id, [FromBody] PacienteSistemaDTO pacienteDTO)
        {
            try
            {
                var paciente = await _service.UpdatePacienteAsync(id, pacienteDTO);
                return Ok(paciente);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro interno no servidor: {ex.Message}");
            }
        }
    }
}