using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Application.Domain.Converter;

public class ConvertDateTime : IsoDateTimeConverter
{
    public ConvertDateTime() => DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
            writer.WriteNull();
        else
            base.WriteJson(writer, value, serializer);
    }
}