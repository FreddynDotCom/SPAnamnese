using System.ComponentModel.DataAnnotations;

namespace SPAnamnese.Client.DTOs
{
    public class LoginRequestDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}
