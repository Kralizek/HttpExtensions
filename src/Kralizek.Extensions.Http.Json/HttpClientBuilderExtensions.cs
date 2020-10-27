using System;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// A set of extension methods to extend <see cref="IHttpClientBuilder" />.
    /// </summary>
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Adds support for a fluent configuration of <see cref="HttpRestClientOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="IHttpClientBuilder"/>.</param>
        /// <param name="optionsCustomization">A delegate that is used to configure a <see cref="HttpRestClientOptions" />.</param>
        /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
        public static IHttpClientBuilder ConfigureHttpRestClient(this IHttpClientBuilder builder, Action<HttpRestClientOptions> optionsCustomization)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            _ = optionsCustomization ?? throw new ArgumentNullException(nameof(optionsCustomization));

            builder.Services.Configure<HttpRestClientOptions>(builder.Name, optionsCustomization);

            return builder;
        }

        /// <summary>
        /// Adds a delegate that will be used to configure a <see cref="JsonSerializerSettings" />.
        /// </summary>
        /// <param name="builder">The <see cref="IHttpClientBuilder"/>.</param>
        /// <param name="serializationCustomization">A delegate that is used to configure a <see cref="JsonSerializerSettings" />.</param>
        /// <returns></returns>
        public static IHttpClientBuilder ConfigureSerialization(this IHttpClientBuilder builder, Action<JsonSerializerSettings> serializationCustomization)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            _ = serializationCustomization ?? throw new ArgumentNullException(nameof(serializationCustomization));

            return ConfigureHttpRestClient(builder, options => serializationCustomization(options.SerializerSettings));
        }
    }
}
