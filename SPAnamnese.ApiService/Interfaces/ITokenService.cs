using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Models;

namespace SPAnamnese.ApiService.Interfaces
{
    public interface ITokenService
    {
        (string token, DateTime expiraEm) GerarAccessToken(tbusuario usuario);
    }
}
