using Microsoft.AspNetCore.Mvc;
using SPAnamnese.ApiService.Interfaces;

namespace SPAnamnese.ApiService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TesteController : ControllerBase
    {
        private readonly ITeste _service;

        public TesteController(ITeste service)
        {
            _service = service;
        }

        [HttpGet("TesteById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var paciente = await _service.GetById(id);

            if (paciente is null) NotFound();

            return Ok(paciente);
        }
    }
}
