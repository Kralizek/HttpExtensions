using System;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using Kralizek.Extensions.Http;
using NUnit.Framework;

namespace Tests.Extensions.Http
{
    [TestFixture]
    public class HttpQueryStringBuilderTests
    {

        [Test, CustomAutoData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HttpQueryStringBuilder).GetConstructors());
        }

        [Test, CustomAutoData]
        public void HasKey_returns_false_if_empty(HttpQueryStringBuilder sut, string key)
        {
            Assert.That(sut.HasKey(key), Is.False);
        }

        [Test, CustomAutoData]
        public void HasKey_returns_true_if_key_is_added(HttpQueryStringBuilder sut, string key, string value)
        {
            sut.Add(key, value);

            Assert.That(sut.HasKey(key), Is.True);
        }

        [Test, CustomAutoData]
        public void Same_key_can_be_added_more_than_once(HttpQueryStringBuilder sut, string key, string[] values)
        {
            foreach (var value in values)
            {
                sut.Add(key, value);
            }
        }

        [Test, CustomAutoData]
        public void Add_throws_if_key_is_null(HttpQueryStringBuilder sut, string value)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Add(null, value));
        }

        [Test, CustomAutoData]
        public void Add_throws_if_value_is_null(HttpQueryStringBuilder sut, string key)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Add(key, null));
        }

        [Test, CustomAutoData]
        public void QueryString_can_be_implicitly_converted_to_string(HttpQueryStringBuilder sut)
        {
            var query = sut.BuildQuery();

            Assume.That(query, Is.InstanceOf<QueryString>());

            string querystring = query;

            Assert.That(querystring, Is.Not.Null);
        }

        [Test, CustomAutoData]
        public void BuildQuery_returns_query_with_added_items(HttpQueryStringBuilder sut, string[] keys, string[] values)
        {
            Assume.That(keys.Length, Is.EqualTo(values.Length));

            for (int i = 0; i < keys.Length; i++)
            {
                sut.Add(keys[i], values[i]);
            }

            string querystring = sut.BuildQuery();

            for (int i = 0; i < keys.Length; i++)
            {
                Assert.That(querystring, Contains.Substring($"{keys[i]}={values[i]}"));
            }
        }
    }
}
