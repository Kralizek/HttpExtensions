using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kralizek.Extensions.Http
{
    public class JsonContent : StringContent
    {
        public JsonContent(string content, Encoding encoding, string mediaType = "application/json") : base(content, encoding, mediaType) { }

        public JsonContent(string content, string mediaType = "application/json") : base (content, Encoding.UTF8, mediaType) { }

        public static HttpContent FromObject<T>(T @object, JsonSerializerSettings settings)
        {
            string serialized = JsonConvert.SerializeObject(@object, settings);
            return new JsonContent(serialized);
        }

        public static HttpContent FromObject<T>(T @object) => FromObject(@object,  JsonConvert.DefaultSettings?.Invoke());

        public static HttpContent EmptyObject => new JsonContent("{}");

        public static HttpContent EmptyArray => new JsonContent("[]");
    }

    public static class JsonContentExtensions 
    {
        public static async Task<T> ReadAs<T>(this JsonContent content, JsonSerializerSettings settings)
        {
            var value = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(value, settings);
        }

        public static Task<T> ReadAs<T>(this JsonContent content) => ReadAs<T>(content, JsonConvert.DefaultSettings?.Invoke());
    }
}
