using SPAnamnese.ApiService.Models;
using System.ComponentModel.DataAnnotations;

namespace SPAnamnese.ApiService.DTOs
{
    public class RegisterRequestDTO
    {
        [Required, MinLength(3)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Senha { get; set; } = string.Empty;

        /// <summary>
        /// Papel desejado: (ver <see cref="Roles"/>).
        /// </summary>
        [Required]
        public string Role { get; set; } = Roles.Funcionario;
    }
}
