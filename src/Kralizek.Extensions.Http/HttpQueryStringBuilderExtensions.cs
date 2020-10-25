using System;
using System.Globalization;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// A set of convenient extension methods to <see cref="HttpQueryStringBuilder" />.
    /// </summary>
    public static class HttpQueryStringBuilderExtensions
    {
        /// <summary>
        /// Adds a fragment whose value is a <see cref="bool" />.
        /// </summary>
        /// <param name="builder">The extended <see cref="HttpQueryStringBuilder" />.</param>
        /// <param name="fieldName">The key of the fragment to add.</param>
        /// <param name="value">The value of the fragment to add. The value will be specified in lowercase (<c>true</c> and <c>false</c>).</param>
        public static void Add(this HttpQueryStringBuilder builder, string fieldName, bool value)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            builder.Add(fieldName, value ? "true" : "false");
        }

        /// <summary>
        /// Adds a fragment whose value implements the <see cref="IConvertible" /> interface.
        /// </summary>
        /// <param name="builder">The extended <see cref="HttpQueryStringBuilder" />.</param>
        /// <param name="fieldName">The key of the fragment to add.</param>
        /// <param name="value">The value of the fragment to add.</param>
        /// <typeparam name="T">The type of the value of the fragment. It must implement the <see cref="IConvertible" /> interface.</typeparam>
        public static void Add<T>(this HttpQueryStringBuilder builder, string fieldName, T value)
            where T : IConvertible
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            builder.Add(fieldName, (string)Convert.ChangeType(value, typeof(string), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds a fragment whose value is a <c>struct</c> implementing the <see cref="IConvertible" /> interface.
        /// The fragment will not be added if the value is <c>null</c>.
        /// </summary>
        /// <param name="builder">The extended <see cref="HttpQueryStringBuilder" />.</param>
        /// <param name="fieldName">The key of the fragment to add.</param>
        /// <param name="value">The value of the fragment to add.</param>
        /// <typeparam name="T">The type of the value of the fragment. It must be a <c>struct</c> implementing the <see cref="IConvertible" /> interface.</typeparam>
        public static void Add<T>(this HttpQueryStringBuilder builder, string fieldName, T? value)
            where T : struct, IConvertible
        {
            if (value.HasValue)
            {
                builder.Add(fieldName, value.Value);
            }
        }
    }
}
