using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Kralizek.Extensions.Http;
using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.Extensions.Http
{
    [TestFixture]
    public class HttpRestClientServiceCollectionExtensionsTests
    {
        [Test, CustomAutoData]
        public void AddHttpRestClient_returns_same_services(ServiceCollection services, string configurationName, Action<HttpClient> clientConfiguration)
        {
            var result = services.AddHttpRestClient(configurationName, clientConfiguration);

            Assert.That(result, Is.SameAs(services));
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_returns_same_services(ServiceCollection services, Action<HttpClient> clientConfiguration)
        {
            var result = services.AddHttpRestClient(clientConfiguration);

            Assert.That(result, Is.SameAs(services));
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_adds_logging(ServiceCollection services, string configurationName, Action<HttpClient> clientConfiguration)
        {
            services.AddHttpRestClient(configurationName, clientConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<HttpRestClient>>();

            Assert.That(logger, Is.Not.Null);
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_adds_logging(ServiceCollection services, Action<HttpClient> clientConfiguration)
        {
            services.AddHttpRestClient(clientConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<HttpRestClient>>();

            Assert.That(logger, Is.Not.Null);
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_adds_options(ServiceCollection services, string configurationName, Action<HttpClient> clientConfiguration)
        {
            services.AddHttpRestClient(configurationName, clientConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var options = serviceProvider.GetRequiredService<IOptions<HttpRestClientOptions>>();

            Assert.That(options, Is.Not.Null);
            Assert.That(options.Value, Is.Not.Null);
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_adds_options(ServiceCollection services, Action<HttpClient> clientConfiguration)
        {
            services.AddHttpRestClient(clientConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var options = serviceProvider.GetRequiredService<IOptions<HttpRestClientOptions>>();

            Assert.That(options, Is.Not.Null);
            Assert.That(options.Value, Is.Not.Null);
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_configure_options(ServiceCollection services, string configurationName, Action<HttpClient> clientConfiguration, Action<HttpRestClientOptions> optionsConfiguration)
        {
            services.AddHttpRestClient(configurationName, clientConfiguration, optionsConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var snapshot = serviceProvider.GetRequiredService<IOptionsSnapshot<HttpRestClientOptions>>();

            var options = snapshot.Get(configurationName);

            Mock.Get(optionsConfiguration).Verify(p => p(options), Times.Once());
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_configure_options(ServiceCollection services, Action<HttpClient> clientConfiguration, Action<HttpRestClientOptions> optionsConfiguration)
        {
            services.AddHttpRestClient(clientConfiguration, optionsConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var snapshot = serviceProvider.GetRequiredService<IOptionsSnapshot<HttpRestClientOptions>>();

            var options = snapshot.Get(HttpRestClientServiceCollectionExtensions.HttpRestClientDefaultConfigurationName);

            Mock.Get(optionsConfiguration).Verify(p => p(options), Times.Once());
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_adds_httpClientFactory(ServiceCollection services, string configurationName, Action<HttpClient> clientConfiguration)
        {
            services.AddHttpRestClient(configurationName, clientConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            Assert.That(httpClientFactory, Is.Not.Null);
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_adds_httpClientFactory(ServiceCollection services, Action<HttpClient> clientConfiguration)
        {
            services.AddHttpRestClient(clientConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            Assert.That(httpClientFactory, Is.Not.Null);
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_configures_httpClientFactory(ServiceCollection services, string configurationName, Action<HttpClient> clientConfiguration)
        {
            services.AddHttpRestClient(configurationName, clientConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            var httpClient = httpClientFactory.CreateClient(configurationName);

            Mock.Get(clientConfiguration).Verify(p => p(httpClient));
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_configures_httpClientFactory(ServiceCollection services, Action<HttpClient> clientConfiguration)
        {
            services.AddHttpRestClient(clientConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            var httpClient = httpClientFactory.CreateClient(HttpRestClientServiceCollectionExtensions.HttpRestClientDefaultConfigurationName);

            Mock.Get(clientConfiguration).Verify(p => p(httpClient));
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_correctly_configures_HttpRestClient(ServiceCollection services, string configurationName, Action<HttpClient> clientConfiguration, Action<HttpRestClientOptions> optionsConfiguration)
        {
            services.AddHttpRestClient(configurationName, clientConfiguration, optionsConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<IHttpRestClient>();

            Assert.That(client, Is.Not.Null);
        }

        [Test, CustomAutoData]
        public void AddHttpRestClient_correctly_configures_HttpRestClient(ServiceCollection services, Action<HttpClient> clientConfiguration, Action<HttpRestClientOptions> optionsConfiguration)
        {
            services.AddHttpRestClient(clientConfiguration, optionsConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<IHttpRestClient>();

            Assert.That(client, Is.Not.Null);
        }
    }
}
