namespace SPAnamnese.Client.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        public string NomeCompleto { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string SenhaHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Paciente";

        public bool Ativo { get; set; } = true;

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}