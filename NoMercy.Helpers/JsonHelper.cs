using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoMercy.Helpers;

public static class JsonHelper
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Formatting = Formatting.Indented,
        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore,

        TypeNameHandling = TypeNameHandling.None,
        
        Error = (_, ev) =>
        {
            ev.ErrorContext.Handled = true;
        },
        
        Converters =
        {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
            new ParseNumbersAsInt32Converter()
        }
    };

    public static T? FromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, Settings);
    }

    public static string ToJson<T>(this T self)
    {
        return JsonConvert.SerializeObject(self, Settings);
    }
    
}
public class ParseNumbersAsInt32Converter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(long) || objectType == typeof(long?) || objectType == typeof(object);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        return reader.Value is long 
            ? Convert.ToInt64(reader.Value ?? 0) 
            : reader.Value;
    }
}
