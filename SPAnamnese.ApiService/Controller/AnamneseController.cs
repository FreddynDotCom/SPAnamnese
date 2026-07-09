using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;

namespace SPAnamnese.ApiService.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnamneseController : ControllerBase
    {
        private readonly IAnamnese _service;
        private readonly IMapper _mapper;

        public AnamneseController(IAnamnese service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        ///<summary>
        /// [Anamnese] - Visualizar
        ///</summary>
        /// <returns>Retorna um registro de Anamnese</returns>
        [HttpGet("VisualizarById/{id}")]
        public async Task<IActionResult> VisualizarAnamnese(int id)
        {
            try
            {
                var anamnese = await _service.VisualizarAnamneseAsync(id);
                return Ok(anamnese);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro interno no servidor: {ex.Message}");
            }
        }

        ///<summary>
        /// [Anamnese] - Update
        ///</summary>
        /// <returns>Atualiza um registro de Anamnese</returns>
        [HttpPut("UpdateAnamnese/{id}")]
        public async Task<IActionResult> UpdateAnamnese(int id, [FromBody] AnamneseDTO anamneseDTO)
        {
            try
            {
                var anamnese = await _service.UpdateAnamneseAsync(id, anamneseDTO);
                return Ok(anamnese);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro interno no servidor: {ex.Message}");
            }
        }

        ///<summary>
        /// [Anamnese] - Create
        ///</summary>
        /// <returns>Cria e retorna Anamnese</returns>
        [HttpPost("CreateAnamnese")]
        public async Task<IActionResult> CreateAnamnese([FromBody] AnamneseDTO anamneseDTO)
        {
            try
            {
                var anamnese = await _service.CreateAnamneseAsync(anamneseDTO);
                return Ok(anamnese);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro interno no servidor: {ex.Message}");
            }
        }
    }
}
