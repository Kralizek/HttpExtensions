using System.Net;
using AutoFixture.NUnit3;
using Kralizek.Extensions.Http;
using NUnit.Framework;

namespace Tests.Extensions.Http
{
    public class HttpQueryStringBuilderParseQueryTests
    {
        [Test, AutoData]
        public void Simple_query_can_be_parsed(string key, string value)
        {
            var querystring = $"{key}={value}";

            var builder = HttpQueryStringBuilder.ParseQuery(querystring);

            Assert.That(builder.HasKey(key), Is.True);
        }

        [Test, AutoData]
        public void Question_mark_is_removed(string key, string value)
        {
            var querystring = $"?{key}={value}";

            var builder = HttpQueryStringBuilder.ParseQuery(querystring);

            Assert.That(builder.HasKey(key), Is.True);

        }

        [Test, AutoData]
        public void Query_with_forged_key_can_be_parsed(string keyFirst, string keySecond, string value)
        {
            var key = $"{keyFirst}&{keySecond}";

            var querystring = $"{WebUtility.UrlEncode(key)}={value}";

            var builder = HttpQueryStringBuilder.ParseQuery(querystring);

            Assert.That(builder.HasKey(key), Is.True);
        }

        [Test]
        public void Null_strings_are_sanitized()
        {
            var builder = HttpQueryStringBuilder.ParseQuery(null);

            var query = builder.BuildQuery();

            Assert.That(query.HasItems, Is.False);
        }
    }
}