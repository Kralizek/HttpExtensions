using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Fragment = System.Collections.Generic.KeyValuePair<string, string>;

namespace Kralizek.Extensions.Http
{
    public interface IQueryString
    {
        bool HasItems { get; }

        string Query { get; }
    }

    public class HttpQueryStringBuilder
    {

        public static HttpQueryStringBuilder ParseQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                query = string.Empty;
            }

            if (query.StartsWith("?"))
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

        public HttpQueryStringBuilder() : this(Array.Empty<Fragment>()) { }

        private HttpQueryStringBuilder(IEnumerable<Fragment> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            _inner = new List<Fragment>(items);
        }

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

        public bool HasKey(string key) => _inner.Exists(f => string.Equals(key, f.Key));

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

        public class QueryString : IQueryString
        {
            public QueryString(IReadOnlyList<Fragment> items)
            {
                _items = items ?? throw new ArgumentNullException(nameof(items));
            }

            static string GetQuery(IEnumerable<Fragment> items)
            {
                var queryItems = from item in items
                                 let queryItem = $"{WebUtility.UrlEncode(item.Key)}={WebUtility.UrlEncode(item.Value)}"
                                 select queryItem;

                return string.Join("&", queryItems);
            }

            private readonly IReadOnlyList<Fragment> _items;

            public bool HasItems => _items.Count > 0;

            public string Query => GetQuery(_items);

            public static implicit operator string(QueryString qs) => qs.Query;
        }
    }
}
