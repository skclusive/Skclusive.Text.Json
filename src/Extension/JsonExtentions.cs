using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skclusive.Extensions.DependencyInjection;

namespace Skclusive.Text.Json
{
    public static class JsonExtentions
    {
        public static void TryAddJsonServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IJsonService, JsonService>();
        }

        public static void TryAddJsonConverter<TImplementation>(this IServiceCollection services)
            where TImplementation : JsonConverter
        {
            services.TryAddSingletonEnumerable<JsonConverter, TImplementation>();
        }

         public static void TryAddJsonConverter<TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : JsonConverter
        {
            services.TryAddSingletonEnumerable<JsonConverter, TImplementation>(implementationFactory);
        }

        public static void TryAddJsonTypeConverter<TService, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TService
        {
            services.TryAddSingletonEnumerable<JsonConverter, JsonTypeConverter<TService, TImplementation>>();
        }

        public static void TryAddJsonDictionaryConverter<TKey, TValue>(this IServiceCollection services, Func<string, TKey> parser, Func<IDictionary<TKey, TValue>> source)
        {
            services.TryAddSingletonEnumerable<JsonConverter, JsonDictionaryConverter<TKey, TValue>>(sp => new JsonDictionaryConverter<TKey, TValue>(parser, source));
        }
    }
}