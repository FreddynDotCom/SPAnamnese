using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;

namespace SPAnamnese.ApiService.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string token, DateTime expiraEm) GerarAccessToken(UsuarioDTO usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var chave = jwtSettings["Key"]
                ?? throw new InvalidOperationException("Configuração 'Jwt:Key' não encontrada em appsettings.json.");
            var minutosExpiracao = int.Parse(jwtSettings["AccessTokenExpirationMinutes"] ?? "5");

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.NomeCompleto),
                new(ClaimTypes.Email, usuario.Email),
                new(ClaimTypes.Role, usuario.Role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var credenciais = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave)),
                SecurityAlgorithms.HmacSha256);

            var expiraEm = DateTime.UtcNow.AddMinutes(minutosExpiracao);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiraEm,
                signingCredentials: credenciais);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiraEm);
        }
    }
}