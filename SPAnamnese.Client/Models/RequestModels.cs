namespace SPAnamnese.Client.Models;

/// <summary>Dados enviados ao endpoint de login (POST api/auth/login).</summary>
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

/// <summary>Dados enviados ao endpoint de registro (POST api/auth/registrar).</summary>
public class RegisterRequest
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Role { get; set; } = "Funcionario";
}

/// <summary>Dados enviados aos endpoints de renovação e logout.</summary>
public class RefreshRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
