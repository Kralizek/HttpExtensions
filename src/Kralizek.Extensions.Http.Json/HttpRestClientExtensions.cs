using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// A set of extensions for <see cref="IHttpRestClient" />.
    /// </summary>
    public static class HttpRestClientExtensions
    {
        /// <summary>
        /// Sends an HTTP GET request with payload of type <typeparamref name="TContent"/> and receives a response with payload of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="content">The payload of the request.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TContent">The type of the payload of the request.</typeparam>
        /// <typeparam name="TResult">The type to deserialize the response into.</typeparam>
        /// <returns>If successful, it returns an instance of <typeparamref name="TResult"/>, otherwise it throws a <see cref="HttpException"/>.</returns>
        public static Task<TResult> GetAsync<TContent, TResult>(this IHttpRestClient client, string path, TContent content, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TContent, TResult>(HttpMethod.Get, path, content, query);
        }

        /// <summary>
        /// Sends an HTTP GET request and receives a response with payload of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TResult">The type to deserialize the response into.</typeparam>
        /// <returns>If successful, it returns an instance of <typeparamref name="TResult"/>, otherwise it throws a <see cref="HttpException"/>.</returns>
        public static Task<TResult> GetAsync<TResult>(this IHttpRestClient client, string path, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TResult>(HttpMethod.Get, path, query);
        }

        /// <summary>
        /// Sends an HTTP GET request with payload of type <typeparamref name="TContent"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="content">The payload of the request.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TContent">The type of the payload of the request.</typeparam>
        /// <returns>If unsuccessful, it throws a <see cref="HttpException"/>.</returns>
        public static Task GetAsync<TContent>(this IHttpRestClient client, string path, TContent content, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TContent>(HttpMethod.Get, path, content, query);
        }

        /// <summary>
        /// Sends an HTTP GET request with no payload on both request and response.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <returns>If unsuccessful, it throws a <see cref="HttpException"/>.</returns>
        public static Task GetAsync(this IHttpRestClient client, string path, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync(HttpMethod.Get, path, query);
        }

        /// <summary>
        /// Sends an HTTP POST request with payload of type <typeparamref name="TContent"/> and receives a response with payload of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="content">The payload of the request.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TContent">The type of the payload of the request.</typeparam>
        /// <typeparam name="TResult">The type to deserialize the response into.</typeparam>
        /// <returns>If successful, it returns an instance of <typeparamref name="TResult"/>, otherwise it throws a <see cref="HttpException"/>.</returns>
        public static Task<TResult> PostAsync<TContent, TResult>(this IHttpRestClient client, string path, TContent content, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TContent, TResult>(HttpMethod.Post, path, content, query);
        }

        /// <summary>
        /// Sends an HTTP POST request and receives a response with payload of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TResult">The type to deserialize the response into.</typeparam>
        /// <returns>If successful, it returns an instance of <typeparamref name="TResult"/>, otherwise it throws a <see cref="HttpException"/>.</returns>
        public static Task<TResult> PostAsync<TResult>(this IHttpRestClient client, string path, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TResult>(HttpMethod.Post, path, query);
        }

        /// <summary>
        /// Sends an HTTP POST request with payload of type <typeparamref name="TContent"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="content">The payload of the request.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TContent">The type of the payload of the request.</typeparam>
        /// <returns>If unsuccessful, it throws a <see cref="HttpException"/>.</returns>
        public static Task PostAsync<TContent>(this IHttpRestClient client, string path, TContent content, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TContent>(HttpMethod.Post, path, content, query);
        }

        /// <summary>
        /// Sends an HTTP POST request with no payload on both request and response.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <returns>If unsuccessful, it throws a <see cref="HttpException"/>.</returns>
        public static Task PostAsync(this IHttpRestClient client, string path, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync(HttpMethod.Post, path, query);
        }

        /// <summary>
        /// Sends an HTTP PUT request with payload of type <typeparamref name="TContent"/> and receives a response with payload of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="content">The payload of the request.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TContent">The type of the payload of the request.</typeparam>
        /// <typeparam name="TResult">The type to deserialize the response into.</typeparam>
        /// <returns>If successful, it returns an instance of <typeparamref name="TResult"/>, otherwise it throws a <see cref="HttpException"/>.</returns>
        public static Task<TResult> PutAsync<TContent, TResult>(this IHttpRestClient client, string path, TContent content, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TContent, TResult>(HttpMethod.Put, path, content, query);
        }

        /// <summary>
        /// Sends an HTTP PUT request and receives a response with payload of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TResult">The type to deserialize the response into.</typeparam>
        /// <returns>If successful, it returns an instance of <typeparamref name="TResult"/>, otherwise it throws a <see cref="HttpException"/>.</returns>
        public static Task<TResult> PutAsync<TResult>(this IHttpRestClient client, string path, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TResult>(HttpMethod.Put, path, query);
        }

        /// <summary>
        /// Sends an HTTP PUT request with payload of type <typeparamref name="TContent"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="content">The payload of the request.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TContent">The type of the payload of the request.</typeparam>
        /// <returns>If unsuccessful, it throws a <see cref="HttpException"/>.</returns>
        public static Task PutAsync<TContent>(this IHttpRestClient client, string path, TContent content, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TContent>(HttpMethod.Put, path, content, query);
        }

        /// <summary>
        /// Sends an HTTP PUT request with no payload on both request and response.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <returns>If unsuccessful, it throws a <see cref="HttpException"/>.</returns>
        public static Task PutAsync(this IHttpRestClient client, string path, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync(HttpMethod.Put, path, query);
        }

        /// <summary>
        /// Sends an HTTP DELETE request with payload of type <typeparamref name="TContent"/> and receives a response with payload of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="content">The payload of the request.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TContent">The type of the payload of the request.</typeparam>
        /// <typeparam name="TResult">The type to deserialize the response into.</typeparam>
        /// <returns>If successful, it returns an instance of <typeparamref name="TResult"/>, otherwise it throws a <see cref="HttpException"/>.</returns>
        public static Task<TResult> DeleteAsync<TContent, TResult>(this IHttpRestClient client, string path, TContent content, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TContent, TResult>(HttpMethod.Delete, path, content, query);
        }

        /// <summary>
        /// Sends an HTTP DELETE request and receives a response with payload of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TResult">The type to deserialize the response into.</typeparam>
        /// <returns>If successful, it returns an instance of <typeparamref name="TResult"/>, otherwise it throws a <see cref="HttpException"/>.</returns>
        public static Task<TResult> DeleteAsync<TResult>(this IHttpRestClient client, string path, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TResult>(HttpMethod.Delete, path, query);
        }

        /// <summary>
        /// Sends an HTTP DELETE request with payload of type <typeparamref name="TContent"/>.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="content">The payload of the request.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TContent">The type of the payload of the request.</typeparam>
        /// <returns>If unsuccessful, it throws a <see cref="HttpException"/>.</returns>
        public static Task DeleteAsync<TContent>(this IHttpRestClient client, string path, TContent content, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync<TContent>(HttpMethod.Delete, path, content, query);
        }

        /// <summary>
        /// Sends an HTTP DELETE request with no payload on both request and response.
        /// </summary>
        /// <param name="client">The <see cref="IHttpRestClient" />.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <returns>If unsuccessful, it throws a <see cref="HttpException"/>.</returns>
        public static Task DeleteAsync(this IHttpRestClient client, string path, IQueryString? query = null)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            return client.SendAsync(HttpMethod.Delete, path, query);
        }
    }
}
