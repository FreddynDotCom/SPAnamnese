using System.Text.Json;

namespace SPAnamnese.Client.Services;

/// <summary>
/// Opções de serialização JSON usadas em todas as chamadas HTTP do cliente,
/// garantindo compatibilidade (camelCase, case-insensitive) com o que a API retorna.
/// </summary>
internal static class JsonDefaults
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);
}
