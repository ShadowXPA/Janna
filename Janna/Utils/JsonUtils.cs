using System.Text.Json;

namespace Janna.Utils;

public static class JsonUtils
{
    public static JsonSerializerOptions SNAKE_CASE_OPTIONS { get; } = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
}
