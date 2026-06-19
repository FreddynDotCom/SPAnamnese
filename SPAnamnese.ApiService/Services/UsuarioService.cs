using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPAnamnese.ApiService.Data;
using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;
using SPAnamnese.ApiService.Models;

namespace SPAnamnese.ApiService.Services
{
    public class UsuarioService : IUsuario
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UsuarioService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO?> RegistrarUsuario(RegisterRequestDTO usuario)
        {
            if (usuario.Role != Roles.Administrador && usuario.Role != Roles.Funcionario)
            {
                return null;
            }

            var emailJaExiste = await _db.tbusuario.AnyAsync(u => u.Email == usuario.Email);
            if (emailJaExiste)
            {
                return null;
            }

            var usuarioEntity = new tbusuario
            {
                NomeCompleto = usuario.NomeCompleto,
                Email = usuario.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Senha),
                Role = usuario.Role,
                Ativo = true,
                CriadoEm = DateTime.UtcNow
            };

            _db.tbusuario.Add(usuarioEntity);
            await _db.SaveChangesAsync();

            return _mapper.Map<UsuarioDTO>(usuarioEntity);
        }

        public async Task<UsuarioDTO?> AutenticarUsuario(LoginRequestDTO dto)
        {
            var usuario = await _db.tbusuario.SingleOrDefaultAsync(u => u.Email == dto.Email && u.Ativo);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            {
                return null;
            }

            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<UsuarioDTO?> AtualizarUsuario(int id, UsuarioDTO dto)
        {
            var usuarioEntity = await _db.tbusuario.FirstOrDefaultAsync(u => u.Id == id && u.Ativo);

            if (usuarioEntity is null)
            {
                return null;
            }

            usuarioEntity.NomeCompleto = dto.NomeCompleto;
            usuarioEntity.Email = dto.Email;
            usuarioEntity.Role = dto.Role;

            _db.tbusuario.Update(usuarioEntity);
            await _db.SaveChangesAsync();

            return _mapper.Map<UsuarioDTO>(usuarioEntity);
        }

        public async Task<bool> SoftDeleteUsuario(int id)
        {
            var usuarioEntity = await _db.tbusuario.FirstOrDefaultAsync(u => u.Id == id && u.Ativo);

            if (usuarioEntity is null)
            {
                return false;
            }

            usuarioEntity.Ativo = false;
            _db.tbusuario.Update(usuarioEntity);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}