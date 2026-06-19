using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;

namespace SPAnamnese.ApiService.Controller
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUsuario _service;

        public AuthController(ITokenService tokenService, IUsuario service)
        {
            _tokenService = tokenService;
            _service = service;
        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UsuarioInfoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsuarioInfoDTO>> Registrar(RegisterRequestDTO dto)
        {
            var usuario = await _service.RegistrarUsuario(dto);
            if (usuario == null)
                return BadRequest("Não foi possível registrar o usuário. Verifique os dados e tente novamente.");

            var resultado = new UsuarioInfoDTO
            {
                Id = usuario.Id,
                NomeCompleto = usuario.NomeCompleto,
                Email = usuario.Email,
                Role = usuario.Role
            };

            return CreatedAtAction(nameof(Registrar), resultado);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenResponseDTO>> Login(LoginRequestDTO dto)
        {
            var usuario = await _service.AutenticarUsuario(dto);

            if (usuario is null)
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }

            var resposta = EmitirTokens(usuario);
            return Ok(resposta);
        }

        private TokenResponseDTO EmitirTokens(UsuarioDTO usuario)
        {
            var (accessToken, accessExpiraEm) = _tokenService.GerarAccessToken(usuario);

            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                AccessTokenExpiraEm = accessExpiraEm
            };
        }
    }
}