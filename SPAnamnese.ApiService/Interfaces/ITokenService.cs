using SPAnamnese.ApiService.DTOs;

namespace SPAnamnese.ApiService.Interfaces
{
    public interface ITokenService
    {
        (string token, DateTime expiraEm) GerarAccessToken(UsuarioDTO usuario);
    }
}