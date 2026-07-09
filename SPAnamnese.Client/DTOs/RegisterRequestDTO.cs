using System.ComponentModel.DataAnnotations;

namespace SPAnamnese.Client.DTOs
{
    public class RegisterRequestDTO
    {
        [Required, MinLength(3)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Senha { get; set; } = string.Empty;
    }
}
