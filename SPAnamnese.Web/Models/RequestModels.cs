namespace SPAnamnese.Web.Models
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Role { get; set; } = "Funcionario";
    }
}