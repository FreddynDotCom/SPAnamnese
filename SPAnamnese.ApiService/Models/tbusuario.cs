namespace SPAnamnese.ApiService.Models
{
    public class tbusuario
    {
        public int Id { get; set; }

        public string NomeCompleto { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string SenhaHash { get; set; } = string.Empty;

        public string Role { get; set; } = tbroles.Funcionario;

        public bool Ativo { get; set; } = true;

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
