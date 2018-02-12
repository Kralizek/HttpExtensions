using System;

namespace Kralizek.Extensions.Http
{
    public static class HttpQueryStringBuilderExtensions
    {
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

            builder.Add(fieldName, (string)Convert.ChangeType(value, typeof(string)));
        }

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