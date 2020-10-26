using System;
using System.Net.Http;
using System.Threading.Tasks;
using Kralizek.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleToRequestBin
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddHttpRestClient("RequestBin", http => 
            {
                http.BaseAddress = new Uri("https://localtest.me:8080");
            });
            
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
