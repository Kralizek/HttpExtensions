using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// A set of extension methods used to register the <see cref="HttpRestClient" /> and all its dependencies.
    /// </summary>
    public static class HttpRestClientServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a <see cref="HttpRestClient" /> with a named configuration.
        /// </summary>
        /// <param name="services">An instance of <see cref="IServiceCollection" /> to attach the configuration to.</param>
        /// <param name="configurationName">The name of the configuration to be used for the <see cref="IHttpClientFactory" />.</param>
        /// <param name="configureHttpClient">The configuration of the <see cref="HttpClient" /> returned by the <see cref="IHttpClientFactory" />.</param>
        /// <param name="configureOptions">Optional. Configures the <see cref="HttpRestClientOptions" /> passed to the <see cref="HttpRestClient" />.</param>
        /// <returns>The same instance passed as <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpRestClient(this IServiceCollection services, string configurationName, Action<HttpClient> configureHttpClient, Action<HttpRestClientOptions>? configureOptions = null)
        {
            return AddHttpRestClient(services, configurationName, builder =>
            {
                builder.ConfigureHttpClient(configureHttpClient);

                if (configureOptions != null)
                {
                    builder.ConfigureHttpRestClient(configureOptions);
                }
            });
        }

        /// <summary>
        /// Registers a <see cref="HttpRestClient" /> with a default configuration.
        /// </summary>
        /// <param name="services">An instance of <see cref="IServiceCollection" /> to attach the configuration to.</param>
        /// <param name="configureHttpClient">The configuration of the <see cref="HttpClient" /> returned by the <see cref="IHttpClientFactory" />.</param>
        /// <param name="configureOptions">Optional. Configures the <see cref="HttpRestClientOptions" /> passed to the <see cref="HttpRestClient" />.</param>
        /// <returns>The same instance passed as <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpRestClient(this IServiceCollection services, Action<HttpClient> configureHttpClient, Action<HttpRestClientOptions>? configureOptions = null)
            => AddHttpRestClient(services, HttpRestClientDefaultConfigurationName, configureHttpClient, configureOptions);

        /// <summary>
        /// Registers a <see cref="HttpRestClient" /> with a named configuration.
        /// </summary>
        /// <param name="services">An instance of <see cref="IServiceCollection" /> to attach the configuration to.</param>
        /// <param name="configurationName">The name of the configuration to be used for the <see cref="IHttpClientFactory" />.</param>
        /// <param name="httpClientBuilderCustomization">A delegate to customize an instance of <see cref="IHttpClientBuilder"/>.</param>
        /// <returns>The same instance passed as <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpRestClient(this IServiceCollection services, string configurationName, Action<IHttpClientBuilder> httpClientBuilderCustomization)
        {
            services.AddLogging();

            services.AddOptions();

            services.Configure<HttpRestClientOptions>(configurationName, options => options.HttpClientName = configurationName);

            var httpClientBuilder = services.AddHttpClient(configurationName);

            httpClientBuilderCustomization?.Invoke(httpClientBuilder);

            services.AddTransient<IHttpRestClient>(sp =>
            {
                var snapshot = sp.GetRequiredService<IOptionsSnapshot<HttpRestClientOptions>>();

                var options = snapshot.Get(configurationName);

                var logger = sp.GetRequiredService<ILogger<HttpRestClient>>();

                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();

                var wrapper = new OptionsWrapper<HttpRestClientOptions>(options);

                var service = new HttpRestClient(httpClientFactory, wrapper, logger);

                return service;
            });

            return services;
        }

                /// <summary>
        /// Registers a <see cref="HttpRestClient" /> with a default configuration.
        /// </summary>
        /// <param name="services">An instance of <see cref="IServiceCollection" /> to attach the configuration to.</param>
        /// <param name="httpClientBuilderCustomization">A delegate to customize an instance of <see cref="IHttpClientBuilder"/>.</param>
        /// <returns>The same instance passed as <paramref name="services"/>.</returns>
        public static IServiceCollection AddHttpRestClient(this IServiceCollection services, Action<IHttpClientBuilder> httpClientBuilderCustomization)
            => AddHttpRestClient(services, HttpRestClientDefaultConfigurationName, httpClientBuilderCustomization);

        /// <summary>
        /// The name used when no configuration name is given.
        /// </summary>
        public static readonly string HttpRestClientDefaultConfigurationName = "Default";
    }
}
