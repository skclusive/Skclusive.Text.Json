using System;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Skclusive.Extensions.DependencyInjection;

namespace Skclusive.Text.Json
{
    public static class JsonExtentions
    {
        public static void TryAddJsonConverter<TImplementation>(this IServiceCollection collection)
            where TImplementation : JsonConverter
        {
            collection.TryAddSingletonEnumerable<JsonConverter, TImplementation>();
        }
    }
}