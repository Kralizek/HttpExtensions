using System;
using System.Net.Http;
using System.Threading.Tasks;
using Kralizek.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ConsoleToRequestBin
{
    class Program
    {
        static async Task Main()
        {
            var services = new ServiceCollection();

            services.AddHttpRestClient("RequestBin", builder => builder
                .ConfigureHttpClient(http =>
                {
                    http.BaseAddress = new Uri("https://your-pipe.x.pipedream.net");
                    http.DefaultRequestHeaders.Add("X-Test", "This is a test");
                })
                .ConfigureHttpRestClient(options =>
                {
                    options.ContentMediaType = JsonContent.ApplicationJsonMediaType;
                })
                .ConfigureSerialization(json => 
                {
                    json.ContractResolver = new DefaultContractResolver 
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };

                    json.Formatting = Formatting.Indented;
                }));
            
            var serviceProvider = services.BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<IHttpRestClient>();

            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe"
            };

            await client.SendAsync(HttpMethod.Post, "/v1/person", person).ConfigureAwait(false);
        }
    }

    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
