using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Fragment = System.Collections.Generic.KeyValuePair<string, string>;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// An interface used to represent the querystring part of an HTTP request.
    /// </summary>
    public interface IQueryString
    {
        /// <summary>
        /// Returns <c>true</c> if the current object contains at least one fragment.
        /// </summary>
        bool HasItems { get; }

        /// <summary>
        /// Returns the query string.
        /// </summary>
        string Query { get; }
    }

    /// <summary>
    /// An implementation of <see cref="IQueryString" />.
    /// </summary>
    public class QueryString : IQueryString
    {
        /// <summary>
        /// Creates an instance of <see cref="QueryString" /> with an initial set of fragments.
        /// </summary>
        /// <param name="items">The initial set of fragments to be added to the querystring.</param>
        public QueryString(IReadOnlyList<Fragment> items)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        private static string GetQuery(IEnumerable<Fragment> items)
        {
            var queryItems = from item in items
                                let queryItem = $"{WebUtility.UrlEncode(item.Key)}={WebUtility.UrlEncode(item.Value)}"
                                select queryItem;

            return string.Join("&", queryItems);
        }

        private readonly IReadOnlyList<Fragment> _items;

        /// <inheritdoc/>
        public bool HasItems => _items.Count > 0;

        /// <inheritdoc/>
        public string Query => GetQuery(_items);

        /// <summary>
        /// Implicitly casts the <paramref name="queryString"/> into a <see cref="string"/> by returning its content.
        /// </summary>
        /// <param name="queryString">The instance of <see cref="QueryString"/> to cast into a <see cref="string"/>.</param>
        /// <returns>The content of <paramref name="queryString" />. If <paramref name="queryString"/> is null, <see cref="string.Empty" /> will be returned.</returns>
        public static implicit operator string(QueryString queryString) => queryString?.Query ?? string.Empty;

        /// <summary>
        /// Returns the content of this instance of <see cref="QueryString"/> as a <see cref="string"/> wrapped by curly braces.
        /// </summary>
        public override string ToString()
        {
            return $"{{{this}}}";
        }
    }
}
