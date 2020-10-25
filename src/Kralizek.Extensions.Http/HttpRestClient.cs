using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// An implementation of <see cref="IHttpRestClient" /> that uses <see cref="IHttpClientFactory" /> to provision instances of <see cref="HttpClient"/>.
    /// </summary>
    public class HttpRestClient : IHttpRestClient
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly HttpRestClientOptions _options;
        private readonly ILogger<HttpRestClient> _logger;

        /// <summary>
        /// Initializes an instance of <see cref="HttpRestClient" />.
        /// </summary>
        /// <param name="httpFactory">An instance of <see cref="IHttpClientFactory" /> used to provision instances of <see cref="HttpClient"/>.</param>
        /// <param name="options">An instance of <see cref="HttpRestClientOptions"/> used to customize the behavior of this <see cref="HttpRestClient" />.</param>
        /// <param name="logger">An instance of <see cref="ILogger{HttpRestClient}"/> used to log HTTP requests and responses.</param>
        public HttpRestClient(IHttpClientFactory httpFactory, IOptions<HttpRestClientOptions> options, ILogger<HttpRestClient> logger)
        {
            _httpFactory = httpFactory ?? throw new ArgumentNullException(nameof(httpFactory));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private static string ComposeUrl(string url, IQueryString? query)
        {
            if (query != null && query.HasItems)
            {
                return $"{url}?{query.Query}";
            }

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
#pragma warning disable CA2000 // Dispose objects before losing scope. Justification: False positive, see https://github.com/dotnet/roslyn-analyzers/issues/3042

        /// <summary>
        /// Sends an HTTP request with payload and receives a response with payload.
        /// The request payload will be serialized as JSON.
        /// The response payload is assumed to be JSON.
        /// </summary>
        /// <inheritdoc/>
        public async Task<TResult> SendAsync<TContent, TResult>(HttpMethod method, string path, TContent content, IQueryString? query = null)
        {
            var httpContent = JsonContent.FromObject(content, _options.Encoding, _options.ContentMediaType, _options.SerializerSettings);

            var requestUrl = ComposeUrl(path, query);

            using var request = new HttpRequestMessage(method, requestUrl) { Content = httpContent };

            await LogRequest(request, includeContent: true).ConfigureAwait(false);

            using var response = await CreateClient().SendAsync(request).ConfigureAwait(false);

            await LogResponse(response).ConfigureAwait(false);

            if (response is { IsSuccessStatusCode: true })
            {
                var incomingContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var result = JsonConvert.DeserializeObject<TResult>(incomingContent, _options.SerializerSettings);

                return result;
            }
            else
            {
                throw new HttpException(response.StatusCode, response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Sends an HTTP request and receives a response with payload.
        /// The response payload is assumed to be JSON.
        /// </summary>
        /// <inheritdoc/>
        public async Task<TResult> SendAsync<TResult>(HttpMethod method, string path, IQueryString? query = null)
        {
            var requestUrl = ComposeUrl(path, query);

            using var request = new HttpRequestMessage(method, requestUrl);

            await LogRequest(request).ConfigureAwait(false);

            using var response = await CreateClient().SendAsync(request).ConfigureAwait(false);

            await LogResponse(response).ConfigureAwait(false);

            if (response is { IsSuccessStatusCode: true })
            {
                var incomingContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var result = JsonConvert.DeserializeObject<TResult>(incomingContent, _options.SerializerSettings);

                return result;
            }
            else
            {
                throw new HttpException(response.StatusCode, response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Sends an HTTP request with payload.
        /// The request payload will be serialized as JSON.
        /// </summary>
        /// <inheritdoc/>
        public async Task SendAsync<TContent>(HttpMethod method, string path, TContent content, IQueryString? query = null)
        {
            var httpContent = JsonContent.FromObject(content, _options.Encoding, _options.ContentMediaType, _options.SerializerSettings);

            var requestUrl = ComposeUrl(path, query);

            using var request = new HttpRequestMessage(method, requestUrl) { Content = httpContent };

            await LogRequest(request, includeContent: true).ConfigureAwait(false);

            using var response = await CreateClient().SendAsync(request).ConfigureAwait(false);

            await LogResponse(response).ConfigureAwait(false);

            if (response is { IsSuccessStatusCode: false })
            {
                throw new HttpException(response.StatusCode, response.ReasonPhrase);
            }
        }

        /// <inheritdoc/>
        public async Task SendAsync(HttpMethod method, string path, IQueryString? query = null)
        {
            var requestUrl = ComposeUrl(path, query);

            using var request = new HttpRequestMessage(method, requestUrl);

            await LogRequest(request, includeContent: true).ConfigureAwait(false);

            using var response = await CreateClient().SendAsync(request).ConfigureAwait(false);

            await LogResponse(response).ConfigureAwait(false);

            if (response is { IsSuccessStatusCode: false })
            {
                throw new HttpException(response.StatusCode, response.ReasonPhrase);
            }
        }

#pragma warning restore CA2000
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
            [HttpMethod.Trace] = new EventId(1007, HttpMethod.Head.Method),
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
                        response.RequestMessage.Method.Method.ToUpper(CultureInfo.InvariantCulture),
                        response.RequestMessage.RequestUri.PathAndQuery,
                        response.StatusCode.ToString("D"),
                        response.ReasonPhrase,
                        responseContent);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    _logger.LogError(eventId, ex, "{METHOD}: {PATHANDQUERY} {STATUS} {REASON} {ERRORMESSAGE}",
                        response.RequestMessage.Method.Method.ToUpper(CultureInfo.InvariantCulture),
                        response.RequestMessage.RequestUri.PathAndQuery,
                        response.StatusCode.ToString("D"),
                        response.ReasonPhrase,
                        ex.Message);
                }
#pragma warning restore CA1031
            }
            else
            {
                _logger.LogDebug(eventId, "{METHOD}: {PATHANDQUERY} {STATUS} '{REASON}'",
                    response.RequestMessage.Method.Method.ToUpper(CultureInfo.InvariantCulture),
                    response.RequestMessage.RequestUri.PathAndQuery,
                    response.StatusCode.ToString("D"),
                    response.ReasonPhrase);
            }
        }

        private async Task LogRequest(HttpRequestMessage request, bool includeContent = false)
        {
            var eventId = GetEventId();

            var content = await GetContent().ConfigureAwait(false);

            _logger.LogDebug(eventId, $"{{METHOD}}: {{REQUESTURI}} {{TYPECONTENT}} {(includeContent ? "{CONTENT}" : string.Empty)}",
                request.Method.Method.ToUpper(CultureInfo.InvariantCulture),
                request.RequestUri,
                request.Content?.GetType().Name,
                content);

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
                    return await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                }

                return null;
            }
        }

        #endregion
    }
}
