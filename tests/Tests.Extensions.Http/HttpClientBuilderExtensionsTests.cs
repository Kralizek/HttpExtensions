using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Kralizek.Extensions.Http;
using System;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;

namespace Tests.Extensions.Http
{
    [TestFixture]
    public class HttpClientBuilderExtensionsTests
    {
        [Test, CustomAutoData]
        public void ConfigureHttpRestClient_attaches_configuration(ServiceCollection services, string configurationName, Action<HttpRestClientOptions> configuration)
        {
            services.AddHttpClient(configurationName).ConfigureHttpRestClient(configuration);

            var serviceProvider = services.BuildServiceProvider();

            var snapshot = serviceProvider.GetRequiredService<IOptionsSnapshot<HttpRestClientOptions>>();

            var options = snapshot.Get(configurationName);

            Mock.Get(configuration).Verify(p => p(options));
        }

        [Test, CustomAutoData]
        public void ConfigureSerialization_attaches_configuration(ServiceCollection services, string configurationName, Action<JsonSerializerSettings> configuration)
        {
            services.AddHttpClient(configurationName).ConfigureSerialization(configuration);

            var serviceProvider = services.BuildServiceProvider();

            var snapshot = serviceProvider.GetRequiredService<IOptionsSnapshot<HttpRestClientOptions>>();

            var options = snapshot.Get(configurationName);

            Mock.Get(configuration).Verify(p => p(options.SerializerSettings));
        }
    }

}
