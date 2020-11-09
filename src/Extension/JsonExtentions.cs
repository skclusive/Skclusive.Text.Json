using System;
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
    }
}