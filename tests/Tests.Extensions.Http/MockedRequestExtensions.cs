using Newtonsoft.Json;
using RichardSzalay.MockHttp;

namespace Tests.Extensions.Http
{
    public static class MockedRequestExtensions
    {
        public static MockedRequest WithJsonContent<T> (this MockedRequest request, T content, JsonSerializerSettings settings = null)
        {
            var json = JsonConvert.SerializeObject(content, settings);

            return request.WithContent(json);
        }
    }
}