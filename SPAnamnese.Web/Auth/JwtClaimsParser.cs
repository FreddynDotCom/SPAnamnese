using System.Security.Claims;
using System.Text.Json;

namespace SPAnamnese.Web.Auth
{
    /// <summary>
    /// Decodifica o payload de um JWT e converte suas claims em objetos
    /// Claim que o Blazor entende (AuthorizeView, [Authorize], etc.).
    /// </summary>
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
}