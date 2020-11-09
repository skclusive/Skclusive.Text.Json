using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Skclusive.Text.Json
{
    public class JsonService : IJsonService
    {
        public JsonService(IEnumerable<JsonConverter> converters)
        {
            SerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            foreach(var converter in converters)
            {
                SerializerOptions.Converters.Add(converter);
            }
        }

        private JsonSerializerOptions SerializerOptions { get; }

        public string Serialize(object value)
        {
            return JsonSerializer.Serialize(value, SerializerOptions);
        }

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, SerializerOptions);
        }
    }
}
