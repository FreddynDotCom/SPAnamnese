using System.Security.Claims;
using System.Text.Json;

namespace SPAnamnese.Client.Auth;

/// <summary>
/// Decodifica o "payload" (parte central) de um JWT e converte suas
/// claims em objetos <see cref="Claim"/> que o Blazor entende.
/// </summary>
/// <remarks>
/// Importante: o backend cria o token usando diretamente as constantes
/// <c>ClaimTypes.Role</c>, <c>ClaimTypes.Name</c>, etc. como chaves do JSON.
/// Por isso, ao decodificar aqui, as claims já saem com o <c>Claim.Type</c>
/// correto (ex.: a URI longa de <c>ClaimTypes.Role</c>), e os componentes
/// como <c>&lt;AuthorizeView Roles="Administrador"&gt;</c> funcionam
/// sem nenhuma configuração extra.
/// </remarks>
public static class JwtClaimsParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = ExtrairPayload(jwt);
        var bytes = Base64UrlDecode(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(bytes)
            ?? new Dictionary<string, object>();

        var claims = new List<Claim>();

        foreach (var kvp in keyValuePairs)
        {
            // Algumas claims podem vir como array (ex.: múltiplas roles).
            if (kvp.Value is JsonElement { ValueKind: JsonValueKind.Array } arrayElement)
            {
                foreach (var item in arrayElement.EnumerateArray())
                {
                    claims.Add(new Claim(kvp.Key, item.ToString()));
                }
            }
            else
            {
                claims.Add(new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
            }
        }

        return claims;
    }

    private static string ExtrairPayload(string jwt)
    {
        var partes = jwt.Split('.');
        if (partes.Length < 2)
        {
            throw new ArgumentException("Token JWT em formato inválido.", nameof(jwt));
        }
        return partes[1];
    }

    /// <summary>
    /// Decodifica uma string Base64Url (usada em JWT), ajustando o
    /// padding ('=') que o formato Base64Url omite.
    /// </summary>
    private static byte[] Base64UrlDecode(string base64Url)
    {
        var base64 = base64Url.Replace('-', '+').Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }
}
