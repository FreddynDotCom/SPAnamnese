namespace SPAnamnese.Web.Models
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiraEm { get; set; }
    }
}