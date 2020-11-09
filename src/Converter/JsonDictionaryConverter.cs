using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Skclusive.Text.Json
{
    public abstract class JsonBaseDictionaryConverter<K, V> : JsonConverter<IDictionary<K, V>>
    {
        public override IDictionary<K, V> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dictionary = GetSource();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dictionary;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("JsonTokenType was not PropertyName");
                }

                K key = ReadKey(ref reader, typeof(K), options);

                reader.Read();

                V value = JsonSerializer.Deserialize<V>(ref reader, options);

                dictionary.Add(key, value);
            }

            throw new JsonException("Error Occured");
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<K, V> dictionary, JsonSerializerOptions options)
        {
            if (dictionary == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();

            foreach (KeyValuePair<K, V> item in dictionary)
            {
                writer.WritePropertyName(item.Key.ToString());

                // string key = JsonSerializer.Serialize(item.Key, typeof(K), options);
                // string value = JsonSerializer.Serialize(item.Value, typeof(V), options);
                // writer.WriteString(key, value);

                JsonSerializer.Serialize(writer, item.Value, options);
            }

            writer.WriteEndObject();
            // JsonSerializer.Serialize(writer, new Dictionary<K, V>(dictionary), options);
        }

        protected abstract IDictionary<K, V> GetSource();

        protected abstract K ReadKey(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);
    }

    public class JsonStringDictionaryConverter<V> : JsonBaseDictionaryConverter<string, V>
    {
        protected override IDictionary<string, V> GetSource()
        {
            return new Dictionary<string, V>();
        }

        protected override string ReadKey(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString();
        }
    }

    public class JsonIntDictionaryConverter<V> : JsonBaseDictionaryConverter<int, V>
    {
        protected override IDictionary<int, V> GetSource()
        {
            return new Dictionary<int, V>();
        }

        protected override int ReadKey(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return int.Parse(reader.GetString());
        }
    }

    public class JsonGuidDictionaryConverter<V> : JsonBaseDictionaryConverter<Guid, V>
    {
        protected override IDictionary<Guid, V> GetSource()
        {
            return new Dictionary<Guid, V>();
        }

        protected override Guid ReadKey(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Guid.Parse(reader.GetString());
        }
    }

    public class JsonDictionaryConverter<K, V> : JsonBaseDictionaryConverter<K, V>
    {
        public JsonDictionaryConverter(Func<string, K> parser, Func<IDictionary<K, V>> source)
        {
            Parser = parser;

            Source = source ?? (() => new Dictionary<K, V>());
        }

        public Func<string, K> Parser { get; }

        public Func<IDictionary<K, V>> Source { get; }

        protected override IDictionary<K, V> GetSource()
        {
            return Source();
        }

        protected override K ReadKey(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Parser(reader.GetString());
        }
    }
}
