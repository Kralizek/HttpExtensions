using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Fragment = System.Collections.Generic.KeyValuePair<string, string>;

namespace Kralizek.Extensions.Http
{
    /// <summary>
    /// Helper class that can be used to create a valid instance of <see cref="IQueryString"/>.
    /// </summary>
    public class HttpQueryStringBuilder
    {
        /// <summary>
        /// Creates a <see cref="HttpQueryStringBuilder" /> from any string trying to parse and decode its content.
        /// </summary>
        /// <param name="query">The string to parse.</param>
        /// <returns>A valid <see cref="HttpQueryStringBuilder" /> containing all the fragments parsed from the <paramref name="query"/>.</returns>
        public static HttpQueryStringBuilder ParseQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                query = string.Empty;
            }

            if (query.StartsWith("?", StringComparison.Ordinal))
            {
                query = query.Substring(1);
            }

            var items = from fragment in query.Split('&')
                        where fragment.Length > 0
                        let pieces = fragment.Split('=')
                        let key = WebUtility.UrlDecode(pieces[0])
                        let value = WebUtility.UrlDecode(pieces[1])
                        select new Fragment(key, value);

            return new HttpQueryStringBuilder(items);
        }

        private readonly List<Fragment> _inner;

        /// <summary>
        /// Creates an empty <see cref="HttpQueryStringBuilder" />.
        /// </summary>
        public HttpQueryStringBuilder()
            : this(Array.Empty<Fragment>())
        {
        }

        private HttpQueryStringBuilder(IEnumerable<Fragment> items)
        {
            _ = items ?? throw new ArgumentNullException(nameof(items));

            _inner = new List<Fragment>(items);
        }

        /// <summary>
        /// Adds a fragment to the querystring being built.
        /// </summary>
        /// <param name="key">The key of the fragment.</param>
        /// <param name="value">The value of the fragment.</param>
        public void Add(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            _inner.Add(new Fragment(key, value));
        }

        /// <summary>
        /// Checks whether the <paramref name="key"/> has already been added.
        /// </summary>
        /// <param name="key">The fragment key to check.</param>
        /// <returns>Returns <c>true</c> if a fragment with the same key has already been added. The <see cref="StringComparison.Ordinal"/> will be used for the comparison.</returns>
        public bool HasKey(string key) => _inner.Exists(f => string.Equals(key, f.Key, StringComparison.Ordinal));

        /// <summary>
        /// Builds a valid instance of <see cref="IQueryString" /> with all the querystring fragments added to the builder.
        /// </summary>
        /// <param name="sortKeys">Specifies whether or not fragments should be sorted by their key. The <see cref="StringComparer.OrdinalIgnoreCase" /> will be used for sorting.</param>
        /// <param name="collateKeysBy">Specifies whether fragments with the same key should be collated by specifying the separator string to be used.</param>
        /// <returns>A valid instance of <see cref="IQueryString" /> with all the querystring fragments added to the builder.</returns>
        public QueryString BuildQuery(bool sortKeys = true, string? collateKeysBy = null)
        {
            IEnumerable<Fragment> items = _inner;

            if (!string.IsNullOrEmpty(collateKeysBy))
            {
                items = items.GroupBy(i => i.Key, e => e.Value)
                             .Select(g => new Fragment(g.Key, string.Join(collateKeysBy, g)));
            }

            if (sortKeys)
            {
                items = items.OrderBy(i => i.Key, StringComparer.OrdinalIgnoreCase);
            }

            return new QueryString(items.ToArray());
        }
    }
}
