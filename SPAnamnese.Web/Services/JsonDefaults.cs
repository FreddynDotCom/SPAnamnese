using System.Text.Json;

namespace SPAnamnese.Web.Services
{
    internal static class JsonDefaults
    {
        public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);
    }

}
