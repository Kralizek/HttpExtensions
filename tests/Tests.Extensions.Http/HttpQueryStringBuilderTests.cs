using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using AutoFixture.NUnit3;
using Kralizek.Extensions.Http;
using NUnit.Framework;
using Fragment = System.Collections.Generic.KeyValuePair<string, string>;

namespace Tests.Extensions.Http
{
    [TestFixture]
    public class HttpQueryStringBuilderTests
    {
        private IFixture _fixture;

        [SetUp]
        public void Initialize()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void Has_parameterless_constructor()
        {
            var sut = new HttpQueryStringBuilder();
        }

        private HttpQueryStringBuilder CreateSystemUnderTest() => new HttpQueryStringBuilder();

        [Test, AutoData]
        public void HasKey_returns_false_if_empty(string key)
        {
            var sut = CreateSystemUnderTest();

            Assert.That(sut.HasKey(key), Is.False);
        }

        [Test, AutoData]
        public void HasKey_returns_true_if_key_is_added(string key, string value)
        {
            var sut = CreateSystemUnderTest();

            sut.Add(key, value);

            Assert.That(sut.HasKey(key), Is.True);
        }

        [Test, AutoData]
        public void Same_key_can_be_added_more_than_once(string key, string[] values)
        {
            var sut = CreateSystemUnderTest();

            foreach (var value in values)
            {
                sut.Add(key, value);
            }
        }

        [Test, AutoData]
        public void Add_throws_if_key_is_null(string value)
        {
            var sut = CreateSystemUnderTest();

            Assert.Throws<ArgumentNullException>(() => sut.Add(null, value));
        }

        [Test, AutoData]
        public void Add_throws_if_value_is_null(string key)
        {
            var sut = CreateSystemUnderTest();

            Assert.Throws<ArgumentNullException>(() => sut.Add(key, null));
        }

        [Test]
        public void QueryString_can_be_implicitly_converted_to_string()
        {
            var sut = CreateSystemUnderTest();

            var query = sut.BuildQuery();

            Assume.That(query, Is.InstanceOf<HttpQueryStringBuilder.QueryString>());

            string querystring = query;
        }

        [Test, AutoData]
        public void BuildQuery_returns_query_with_added_items(string[] keys, string[] values)
        {
            var sut = CreateSystemUnderTest();

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
