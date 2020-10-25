using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// A specialization of <see cref="StringContent" /> to handle <c>JSON</c> content.
    /// </summary>
    public class JsonContent : StringContent
    {
        /// <summary>
        /// Returns <c>application/json</c>.
        /// </summary>
        public const string ApplicationJsonMediaType = "application/json";

        /// <summary>
        /// Returns <c>text/json</c>.
        /// </summary>
        public const string TextJsonMediaType = "text/json";

        /// <summary>
        /// Creates an instance of <see cref="JsonContent" />.
        /// </summary>
        /// <param name="content">The JSON payload.</param>
        /// <param name="encoding">The encoding used in the Content-Encoding HTTP header.</param>
        /// <param name="mediaType">The media type part of the Content-Type HTTP header. If not specified <c>application/json</c> is assumed.</param>
        public JsonContent(string content, Encoding encoding, string mediaType = ApplicationJsonMediaType)
            : base(content, encoding, mediaType)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonContent" />. <c>UTF8</c> is assumed.
        /// </summary>
        /// <param name="content">The JSON payload.</param>
        /// <param name="mediaType">The media type part of the Content-Type HTTP header.</param>
        public JsonContent(string content, string mediaType = ApplicationJsonMediaType)
            : this(content, Encoding.UTF8, mediaType)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonContent" /> serializing the given object.
        /// </summary>
        /// <param name="obj">The object to serialize. It cannot be null.</param>
        /// <param name="encoding">The encoding used in the Content-Encoding HTTP header. If not specified <see cref="Encoding.UTF8"/> will be used.</param>
        /// <param name="mediaType">The media type part of the Content-Type HTTP header. If not specified <c>application/json</c> is assumed.</param>
        /// <param name="settings">An instance of <see cref="JsonSerializerSettings" /> used to serialize <paramref name="obj"/>.
        /// If not specified, <see cref="JsonConvert.DefaultSettings"/> will be invoked to create one.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static HttpContent FromObject<T>(T obj, Encoding? encoding = null, string mediaType = ApplicationJsonMediaType, JsonSerializerSettings? settings = null)
        {
            _ = obj ?? throw new ArgumentNullException(nameof(obj));

            settings ??= JsonConvert.DefaultSettings?.Invoke();

            encoding ??= Encoding.UTF8;

            string serialized = JsonConvert.SerializeObject(obj, settings);

            return new JsonContent(serialized, encoding, mediaType);
        }

        /// <summary>
        /// Returns a new instance of <see cref="JsonContent" /> representing an empty object.
        /// </summary>
        public static HttpContent EmptyObject => new JsonContent("{}", Encoding.UTF8, ApplicationJsonMediaType);

        /// <summary>
        /// Returns a new instance of <see cref="JsonContent" /> representing an empty array.
        /// </summary>
        public static HttpContent EmptyArray => new JsonContent("[]", Encoding.UTF8, ApplicationJsonMediaType);
    }
}
