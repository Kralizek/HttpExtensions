using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Kralizek.Extensions.Http
{
    public class HttpRestClientOptions
    {
        public string? HttpClientName { get; set; }

        public JsonSerializerSettings? SerializerSettings { get; set; }
    }

    public interface IHttpRestClient
    {
        Task<TResult> SendAsync<TContent, TResult>(HttpMethod method, string url, TContent content, IQueryString? query = null);
        Task<TResult> SendAsync<TResult>(HttpMethod method, string url, IQueryString? query = null);
        Task SendAsync<TContent>(HttpMethod method, string url, TContent content, IQueryString? query = null);
        Task SendAsync(HttpMethod method, string url, IQueryString? query = null);
    }

    public class HttpRestClient : IHttpRestClient
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly HttpRestClientOptions _options;

        private readonly ILogger<HttpRestClient> _logger;

        public HttpRestClient(IHttpClientFactory httpFactory, IOptions<HttpRestClientOptions> options, ILogger<HttpRestClient> logger)
        {
            _httpFactory = httpFactory ?? throw new ArgumentNullException(nameof(httpFactory));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private static string ComposeUrl(string url, IQueryString? query)
        {
            if (query != null && query.HasItems)
                return $"{url}?{query.Query}";

            return url;
        }

        private HttpClient CreateClient()
        {
            if (_options.HttpClientName != null)
            {
                return _httpFactory.CreateClient(_options.HttpClientName);
            }

            return _httpFactory.CreateClient();
        }

        #region Send

        public async Task<TResult> SendAsync<TContent, TResult>(HttpMethod method, string url, TContent content, IQueryString? query = null)
        {
            var json = JsonConvert.SerializeObject(content, _options.SerializerSettings);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var requestUrl = ComposeUrl(url, query);

            using var request = new HttpRequestMessage(method, requestUrl) { Content = httpContent };

            await LogRequest(request, includeContent: true);

            using var response = await CreateClient().SendAsync(request).ConfigureAwait(false);

            await LogResponse(response);

            if (response is { IsSuccessStatusCode: true })
            {
                var incomingContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var result = JsonConvert.DeserializeObject<TResult>(incomingContent, _options.SerializerSettings);

                return result;
            }
            else
            {
                throw new HttpException(response.StatusCode)
                {
                    ReasonPhrase = response.ReasonPhrase
                };
            }
        }

        public async Task<TResult> SendAsync<TResult>(HttpMethod method, string url, IQueryString? query = null)
        {
            var requestUrl = ComposeUrl(url, query);

            using var request = new HttpRequestMessage(method, requestUrl);
            
            await LogRequest(request);

            using var response = await CreateClient().SendAsync(request).ConfigureAwait(false);
            
            await LogResponse(response);

            if (response is { IsSuccessStatusCode: true })
            {
                var incomingContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var result = JsonConvert.DeserializeObject<TResult>(incomingContent, _options.SerializerSettings);

                return result;
            }
            else
            {
                throw new HttpException(response.StatusCode)
                {
                    ReasonPhrase = response.ReasonPhrase
                };
            }
        }

        public async Task SendAsync<TContent>(HttpMethod method, string url, TContent content, IQueryString? query = null)
        {
            var json = JsonConvert.SerializeObject(content, _options.SerializerSettings);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var requestUrl = ComposeUrl(url, query);

            using var request = new HttpRequestMessage(method, requestUrl) { Content = httpContent };
            
            await LogRequest(request, includeContent: true);

            using var response = await CreateClient().SendAsync(request).ConfigureAwait(false);
            
            await LogResponse(response);

            if (response is { IsSuccessStatusCode: false })
            {
                throw new HttpException(response.StatusCode)
                {
                    ReasonPhrase = response.ReasonPhrase
                };
            }
        }

        public async Task SendAsync(HttpMethod method, string url, IQueryString? query = null)
        {
            var requestUrl = ComposeUrl(url, query);

            using var request = new HttpRequestMessage(method, requestUrl);
            
            await LogRequest(request, includeContent: true);

            using var response = await CreateClient().SendAsync(request).ConfigureAwait(false);
            
            await LogResponse(response);

            if (response is { IsSuccessStatusCode: false })
            {
                throw new HttpException(response.StatusCode)
                {
                    ReasonPhrase = response.ReasonPhrase
                };
            }
        }

        #endregion

        #region Logging

        private static readonly IReadOnlyDictionary<HttpMethod, EventId> HttpMethodEventIds = new Dictionary<HttpMethod, EventId>
        {
            [HttpMethod.Get] = new EventId(1001, HttpMethod.Get.Method),
            [HttpMethod.Post] = new EventId(1002, HttpMethod.Post.Method),
            [HttpMethod.Put] = new EventId(1003, HttpMethod.Put.Method),
            [HttpMethod.Delete] = new EventId(1004, HttpMethod.Delete.Method),
            [HttpMethod.Options] = new EventId(1005, HttpMethod.Options.Method),
            [HttpMethod.Head] = new EventId(1006, HttpMethod.Head.Method),
            [HttpMethod.Trace] = new EventId(1007, HttpMethod.Head.Method)
        };

        private async Task LogResponse(HttpResponseMessage response)
        {
            var eventId = new EventId((int)response.StatusCode, response.ReasonPhrase);

            if (response is { IsSuccessStatusCode: false })
            {
                try
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    _logger.LogError(eventId, "{METHOD}: {PATHANDQUERY} {STATUS} '{REASON}' '{ERROR}'",
                        response.RequestMessage.Method.Method.ToUpper(),
                        response.RequestMessage.RequestUri.PathAndQuery,
                        response.StatusCode.ToString("D"),
                        response.ReasonPhrase,
                        responseContent);
                }
                catch (Exception ex)
                {
                    _logger.LogError(eventId, ex, "{METHOD}: {PATHANDQUERY} {STATUS} {REASON} {ERRORMESSAGE}",
                        response.RequestMessage.Method.Method.ToUpper(),
                        response.RequestMessage.RequestUri.PathAndQuery,
                        response.StatusCode.ToString("D"),
                        response.ReasonPhrase,
                        ex.Message
                    );
                }
            }
            else
            {
                _logger.LogDebug(eventId, "{METHOD}: {PATHANDQUERY} {STATUS} '{REASON}'",
                    response.RequestMessage.Method.Method.ToUpper(),
                    response.RequestMessage.RequestUri.PathAndQuery,
                    response.StatusCode.ToString("D"),
                    response.ReasonPhrase);
            }

        }

        private async Task LogRequest(HttpRequestMessage request, bool includeContent = false)
        {
            var eventId = GetEventId();

            var content = await GetContent();

            _logger.LogDebug(eventId, $"{{METHOD}}: {{REQUESTURI}} {{TYPECONTENT}} {(includeContent ? "{CONTENT}" : "") }",
                request.Method.Method.ToUpper(),
                request.RequestUri,
                request.Content?.GetType().Name,
                content
            );

            EventId GetEventId()
            {
                if (!HttpMethodEventIds.TryGetValue(request.Method, out var value))
                {
                    value = new EventId(1000, request.Method.Method);
                }

                return value;
            }

            async Task<string?> GetContent()
            {
                if (includeContent && request.Content != null)
                {
                    return await request.Content.ReadAsStringAsync();
                }

                return null;
            }
        }

        #endregion
    }
}
