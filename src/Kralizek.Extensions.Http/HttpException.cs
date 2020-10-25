using System;
using System.Net;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// An exception raised when a HTTP request is not successful.
    /// It contains a <see cref="HttpStatusCode"/> and a reason phrase.
    /// </summary>
    [Serializable]
    public class HttpException : Exception
    {
        private const string ErrorMessage = "An error occurred while performing an HTTP request";

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" />.
        /// </summary>
        /// <param name="message">The error message for the exception.</param>
        /// <param name="status">An instance of <see cref="HttpStatusCode" /> returned from the server.</param>
        /// <param name="reasonPhrase">A string containing the reason phrase returned from the server. Optional.</param>
        public HttpException(string message, HttpStatusCode status, string? reasonPhrase = null)
            : this(message)
        {
            StatusCode = status;
            ReasonPhrase = reasonPhrase;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" />.
        /// </summary>
        /// <param name="status">An instance of <see cref="HttpStatusCode" /> returned from the server.</param>
        /// <param name="reasonPhrase">A string containing the reason phrase returned from the server. Optional.</param>
        public HttpException(HttpStatusCode status, string? reasonPhrase = null)
            : this(ErrorMessage, status, reasonPhrase)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" />.
        /// </summary>
        /// <param name="status">An instance of <see cref="HttpStatusCode" /> returned from the server.</param>
        /// <param name="innerException">An inner exception.</param>
        /// <param name="reasonPhrase">A string containing the reason phrase returned from the server. Optional.</param>
        public HttpException(HttpStatusCode status, Exception innerException, string? reasonPhrase = null)
            : this(ErrorMessage, innerException)
        {
            StatusCode = status;
            ReasonPhrase = reasonPhrase;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" />.
        /// </summary>
        public HttpException()
            : this(ErrorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" />.
        /// </summary>
        /// <param name="message">The error message for the exception.</param>
        public HttpException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" />.
        /// </summary>
        public HttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        protected HttpException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// The <see cref="HttpStatusCode" /> returned from the server.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// The reason phrase returned from the server.
        /// </summary>
        public string? ReasonPhrase { get; set; }
    }
}
