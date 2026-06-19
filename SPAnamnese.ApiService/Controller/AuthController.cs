using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPAnamnese.ApiService.Data;
using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;
using SPAnamnese.ApiService.Models;

namespace SPAnamnese.ApiService.Controller
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UsuarioInfoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsuarioInfoDTO>> Registrar(RegisterRequestDTO dto)
        {
            if (dto.Role != Roles.Administrador && dto.Role != Roles.Funcionario)
            {
                return BadRequest($"Role inválida. Use '{Roles.Administrador}' ou '{Roles.Funcionario}'.");
            }

            var emailJaExiste = await _context.tbusuario.AnyAsync(u => u.Email == dto.Email);
            if (emailJaExiste)
            {
                return BadRequest("Já existe um usuário cadastrado com este e-mail.");
            }

            var usuario = new tbusuario
            {
                NomeCompleto = dto.NomeCompleto,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role = dto.Role
            };

            _context.tbusuario.Add(usuario);
            await _context.SaveChangesAsync();

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
            var usuario = await _context.tbusuario.SingleOrDefaultAsync(u => u.Email == dto.Email && u.Ativo);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }

            var resposta = await EmitirTokensAsync(usuario);
            return Ok(resposta);
        }

        private async Task<TokenResponseDTO> EmitirTokensAsync(tbusuario usuario)
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
