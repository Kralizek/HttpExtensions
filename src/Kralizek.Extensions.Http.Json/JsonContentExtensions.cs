using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// A set of convenient extension methods to <see cref="JsonContentExtensions" />.
    /// </summary>
    public static class JsonContentExtensions
    {
        /// <summary>
        /// Reads the content of <paramref name="content"/> and attempts to deserialize it into an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="content">The content part of an HTTP request/response to deserialize.</param>
        /// <param name="settings">An instance of <see cref="JsonSerializerSettings" /> used to deserialize the payload of <paramref name="content"/>.
        /// If not specified, <see cref="JsonConvert.DefaultSettings"/> will be invoked to create one.</param>
        /// <typeparam name="T">The type to deserialize the <paramref name="content"/> into.</typeparam>
        public static async Task<T> ReadAs<T>(this JsonContent content, JsonSerializerSettings? settings = null)
        {
            _ = content ?? throw new ArgumentNullException(nameof(content));

            settings ??= JsonConvert.DefaultSettings?.Invoke();

            var value = await content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}
