using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Kralizek.Extensions.Http;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.Extensions.Http
{
    public class TestClass 
    {
        public string Property {get; set;}
    }

    [TestFixture]
    public class JsonContentTests
    {
        [Test, AutoData]
        public void JsonContent_has_default_contentType(string content)
        {
            var sut = new JsonContent(content);

            Assert.That(sut.Headers.ContentType.MediaType, Is.EqualTo("application/json"));
        }

        [Test, AutoData]
        public void JsonContent_has_default_encoding(string content)
        {
            var sut = new JsonContent(content);

            Assert.That(sut.Headers.ContentType.CharSet, Is.EqualTo(Encoding.UTF8.HeaderName));
        }

        [Test]
        [InlineAutoData("application/json")]
        [InlineAutoData("text/json")]
        public void JsonContent_uses_specified_contentType(string contentType, string content)
        {
            var sut = new JsonContent(content, contentType);

            Assert.That(sut.Headers.ContentType.MediaType, Is.EqualTo(contentType));
        }

        [Test, AutoData]
        public void JsonContent_uses_specified_encoding(Encoding encoding, string content)
        {
            var sut = new JsonContent(content, encoding);

            Assert.That(sut.Headers.ContentType.CharSet, Is.EqualTo(encoding.HeaderName));
        }

        [Test]
        public async Task EmptyObject_can_be_deserialized()
        {
            var content = await JsonContent.EmptyObject.ReadAsStringAsync();
            
            TestClass obj = JsonConvert.DeserializeObject<TestClass>(content);

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Property, Is.Null);
        }

        [Test]
        public async Task EmptyArray_can_be_deserialized()
        {
            var content = await JsonContent.EmptyArray.ReadAsStringAsync();

            TestClass[] objs = JsonConvert.DeserializeObject<TestClass[]>(content);

            Assert.That(objs, Is.Not.Null);
            Assert.That(objs, Is.Empty);
        }

        [Test, AutoData]
        public async Task FromObject_can_be_deserialized(TestClass test)
        {
            var content = await JsonContent.FromObject(test).ReadAsStringAsync();

            TestClass obj = JsonConvert.DeserializeObject<TestClass>(content);

            Assert.That(obj.Property, Is.EqualTo(test.Property));
        }

        [Test, AutoData]
        public async Task ReadAs_can_deserialize(TestClass test)
        {
            var content = JsonConvert.SerializeObject(test);

            var sut = new JsonContent(content);

            var actual = await sut.ReadAs<TestClass>(new JsonSerializerSettings());

            Assert.That(actual.Property, Is.EqualTo(test.Property));
        }

        [Test, AutoData]
        public async Task ReadAs_can_deserialize_with_default_settings(TestClass test)
        {
            var content = JsonConvert.SerializeObject(test);

            var sut = new JsonContent(content);

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings();

            var actual = await sut.ReadAs<TestClass>();

            Assert.That(actual.Property, Is.EqualTo(test.Property));
        }

        [Test, AutoData]
        public async Task ReadAs_can_deserialize_with_no_default_settings(TestClass test)
        {
            var content = JsonConvert.SerializeObject(test);

            var sut = new JsonContent(content);

            var actual = await sut.ReadAs<TestClass>();

            Assert.That(actual.Property, Is.EqualTo(test.Property));
        }
    }
}