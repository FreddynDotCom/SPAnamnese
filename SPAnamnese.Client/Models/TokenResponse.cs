namespace SPAnamnese.Client.Models;

/// <summary>
/// Espelha o TokenResponseDto retornado pela API nos endpoints
/// de login, registro e renovação de token.
/// </summary>
public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiraEm { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiraEm { get; set; }
}
