using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// A class used to customize the behavior of <see cref="HttpRestClient" />.
    /// </summary>
    public class HttpRestClientOptions
    {
        /// <summary>
        /// The name of the <see cref="HttpClient" /> used when invoking <see cref="IHttpClientFactory.CreateClient(string)" />.
        /// </summary>
        public string? HttpClientName { get; set; }

        /// <summary>
        /// The encoding to be used when adding a payload to an HTTP request. Default is <see cref="Encoding.UTF8" />.
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// The media type part of the Content-Type HTTP header used when adding a payload to an HTTP request. Default is <see cref="JsonContent.ApplicationJsonMediaType"/>.
        /// </summary>
        public string ContentMediaType { get; set; } = JsonContent.ApplicationJsonMediaType;

        /// <summary>
        /// An instance of <see cref="JsonSerializerSettings" /> used when serializing and deserializing the content of an HTTP request/message.
        /// </summary>
        public JsonSerializerSettings SerializerSettings { get; set; } = JsonConvert.DefaultSettings?.Invoke() ?? new JsonSerializerSettings();
    }
}
