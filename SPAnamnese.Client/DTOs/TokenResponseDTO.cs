namespace SPAnamnese.Client.DTOs
{
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiraEm { get; set; }
    }
}
