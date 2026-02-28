using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Calendar.Service;

public class StringOrBooleanConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => string.Empty,
            JsonTokenType.False => string.Empty,
            JsonTokenType.String => reader.GetString() ?? string.Empty,
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
        };
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        if (bool.TryParse(value, out var boolValue))
        {
            writer.WriteBooleanValue(boolValue);
        }
        else
        {
            writer.WriteStringValue(value);
        }
    }
}