using SPAnamnese.ApiService.DTOs;

namespace SPAnamnese.ApiService.Interfaces
{
    public interface IUsuario
    {
        Task<UsuarioDTO?> RegistrarUsuario(RegisterRequestDTO usuario);
        Task<UsuarioDTO?> AutenticarUsuario(LoginRequestDTO dto);
        Task<UsuarioDTO?> AtualizarUsuario(int id, UsuarioDTO dto);
        Task<bool> SoftDeleteUsuario(int id);
    }
}