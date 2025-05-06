using Application.Domain.Extension;
using Newtonsoft.Json;

namespace Application.Domain.Converter;

public class EnumToStringConverter<T> : JsonConverter<T?> where T : struct, Enum
{
    public override T? ReadJson(
        JsonReader reader,
        Type objectType,
        T? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer
    )
    {
        string? enumText = reader.Value?.ToString();

        return reader.TokenType == JsonToken.Null || string.IsNullOrWhiteSpace(enumText)
        ? null
            : Enum.TryParse(enumText, true, out T result) ? result : null;
    }

    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        if (value is not null)
            writer.WriteValue(value.Value.GetEnumName());
        else
            writer.WriteNull();
    }
}
