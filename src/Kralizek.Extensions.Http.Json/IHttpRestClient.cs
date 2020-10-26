using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// An opinionated abstraction over HTTP requests.
    /// </summary>
    public interface IHttpRestClient
    {
        /// <summary>
        /// Sends an HTTP request with payload of type <typeparamref name="TContent"/> and receives a response with payload of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="content">The payload of the request.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TContent">The type of the payload of the request.</typeparam>
        /// <typeparam name="TResult">The type to deserialize the response into.</typeparam>
        /// <returns>If successful, it returns an instance of <typeparamref name="TResult"/>, otherwise it throws a <see cref="HttpException"/>.</returns>
        Task<TResult> SendAsync<TContent, TResult>(HttpMethod method, string path, TContent content, IQueryString? query = null);

        /// <summary>
        /// Sends an HTTP request and receives a response with payload of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TResult">The type to deserialize the response into.</typeparam>
        /// <returns>If successful, it returns an instance of <typeparamref name="TResult"/>, otherwise it throws a <see cref="HttpException"/>.</returns>
        /// <returns></returns>
        Task<TResult> SendAsync<TResult>(HttpMethod method, string path, IQueryString? query = null);

        /// <summary>
        /// Sends an HTTP request with payload of type <typeparamref name="TContent"/>.
        /// </summary>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="content">The payload of the request.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <typeparam name="TContent">The type of the payload of the request.</typeparam>
        /// <returns>If unsuccessful, it throws a <see cref="HttpException"/>.</returns>
        Task SendAsync<TContent>(HttpMethod method, string path, TContent content, IQueryString? query = null);

        /// <summary>
        /// Sends an HTTP request.
        /// </summary>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="path">The path to be requested.</param>
        /// <param name="query">The querystring of the request.</param>
        /// <returns>If unsuccessful, it throws a <see cref="HttpException"/>.</returns>
        Task SendAsync(HttpMethod method, string path, IQueryString? query = null);
    }
}
