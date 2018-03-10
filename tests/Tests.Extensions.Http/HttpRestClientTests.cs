using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Kralizek.Extensions.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Extensions.Http
{
    [TestFixture]
    public class HttpRestClientTests
    {
        private IFixture fixture;
        private JsonSerializerSettings serializerSettings;

        [SetUp]
        public void Initialize()
        {
            fixture = new Fixture();
            serializerSettings = new JsonSerializerSettings();
        }

        [Test]
        public void HttpClient_cant_be_null()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpRestClient(null, new JsonSerializerSettings(), Mock.Of<ILogger>()));
        }

        [Test]
        public void JsonSerializerSettings_cant_be_null()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpRestClient(Mock.Of<HttpClient>(), null, Mock.Of<ILogger>()));
        }

        [Test]
        public void Logger_cant_be_null()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpRestClient(Mock.Of<HttpClient>(), new JsonSerializerSettings(), null));
        }

        private HttpRestClient CreateSystemUnderTest(params HttpMessageOptions[] options)
        {
            var messageHandler = new FakeHttpMessageHandler(options);
            var httpClient = new HttpClient(messageHandler);
            return new HttpRestClient(httpClient, serializerSettings, Mock.Of<ILogger>());
        }

        [Test]
        [InlineAutoData("GET")]
        [InlineAutoData("POST")]
        [InlineAutoData("PUT")]
        [InlineAutoData("DELETE")]
        public async Task SendAsync_can_send_request_with_body_and_receive_response_with_body(string method, Uri uri, Request request, Response response)
        {
            var httpMethod = new HttpMethod(method);

            var options = new[]
            {
                new HttpMessageOptions
                {
                    RequestUri = uri,
                    HttpMethod = httpMethod,
                    HttpContent = new StringContent(JsonConvert.SerializeObject(request, serializerSettings)),
                    HttpResponseMessage = new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(response, serializerSettings))
                    }
                }
            };
            var sut = CreateSystemUnderTest(options);

            var actualResponse = await sut.SendAsync<Request, Response>(httpMethod, uri.ToString(), request);

            Assert.That(actualResponse, Is.EqualTo(response));
        }

        [Test]
        [InlineAutoData("GET", HttpStatusCode.NotFound)]
        [InlineAutoData("POST", HttpStatusCode.NotFound)]
        [InlineAutoData("PUT", HttpStatusCode.NotFound)]
        [InlineAutoData("DELETE", HttpStatusCode.NotFound)]
        public void SendAsync_throws_HttpException_if_nonSuccessful_response(string method, HttpStatusCode statusCode, Uri uri, Request request, Response response)
        {
            var httpMethod = new HttpMethod(method);

            var options = new[]
            {
                new HttpMessageOptions
                {
                    RequestUri = uri,
                    HttpMethod = httpMethod,
                    HttpContent = new StringContent(JsonConvert.SerializeObject(request, serializerSettings)),
                    HttpResponseMessage = new HttpResponseMessage(statusCode)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(response, serializerSettings))
                    }
                }
            };

            var sut = CreateSystemUnderTest(options);

            Assert.ThrowsAsync<HttpException>(() => sut.SendAsync<Request, Response>(httpMethod, uri.ToString(), request));
        }

        [Test]
        [InlineAutoData("GET")]
        [InlineAutoData("POST")]
        [InlineAutoData("PUT")]
        [InlineAutoData("DELETE")]
        public async Task SendAsync_can_send_request_with_body_and_receive_response(string method, Uri uri, Request request)
        {
            var httpMethod = new HttpMethod(method);

            var options = new[]
            {
                new HttpMessageOptions
                {
                    RequestUri = uri,
                    HttpMethod = httpMethod,
                    HttpContent = new StringContent(JsonConvert.SerializeObject(request, serializerSettings)),
                    HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                }
            };
            var sut = CreateSystemUnderTest(options);

            await sut.SendAsync(httpMethod, uri.ToString(), request);
        }

        [Test]
        [InlineAutoData("GET", HttpStatusCode.NotFound)]
        [InlineAutoData("POST", HttpStatusCode.NotFound)]
        [InlineAutoData("PUT", HttpStatusCode.NotFound)]
        [InlineAutoData("DELETE", HttpStatusCode.NotFound)]
        public void SendAsync_throws_HttpException_if_nonSuccessful_response(string method, HttpStatusCode statusCode, Uri uri, Request request)
        {
            var httpMethod = new HttpMethod(method);

            var options = new[]
            {
                new HttpMessageOptions
                {
                    RequestUri = uri,
                    HttpMethod = httpMethod,
                    HttpContent = new StringContent(JsonConvert.SerializeObject(request, serializerSettings)),
                    HttpResponseMessage = new HttpResponseMessage(statusCode)
                }
            };

            var sut = CreateSystemUnderTest(options);

            Assert.ThrowsAsync<HttpException>(() => sut.SendAsync<Request>(httpMethod, uri.ToString(), request));
        }

        [Test]
        [InlineAutoData("GET")]
        [InlineAutoData("POST")]
        [InlineAutoData("PUT")]
        [InlineAutoData("DELETE")]
        public async Task SendAsync_can_send_request_and_receive_response_with_body(string method, Uri uri, Response response)
        {
            var httpMethod = new HttpMethod(method);

            var options = new[]
            {
                new HttpMessageOptions
                {
                    RequestUri = uri,
                    HttpMethod = httpMethod,
                    HttpResponseMessage = new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(response, serializerSettings))
                    }
                }
            };
            var sut = CreateSystemUnderTest(options);

            var actualResponse = await sut.SendAsync<Response>(httpMethod, uri.ToString());

            Assert.That(actualResponse, Is.EqualTo(response));
        }

        [Test]
        [InlineAutoData("GET", HttpStatusCode.NotFound)]
        [InlineAutoData("POST", HttpStatusCode.NotFound)]
        [InlineAutoData("PUT", HttpStatusCode.NotFound)]
        [InlineAutoData("DELETE", HttpStatusCode.NotFound)]
        public void SendAsync_throws_HttpException_if_nonSuccessful_response(string method, HttpStatusCode statusCode, Uri uri, Response response)
        {
            var httpMethod = new HttpMethod(method);

            var options = new[]
            {
                new HttpMessageOptions
                {
                    RequestUri = uri,
                    HttpMethod = httpMethod,
                    HttpResponseMessage = new HttpResponseMessage(statusCode)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(response, serializerSettings))
                    }
                }
            };

            var sut = CreateSystemUnderTest(options);

            Assert.ThrowsAsync<HttpException>(() => sut.SendAsync<Response>(httpMethod, uri.ToString()));
        }

        [Test]
        [InlineAutoData("GET")]
        [InlineAutoData("POST")]
        [InlineAutoData("PUT")]
        [InlineAutoData("DELETE")]
        public async Task SendAsync_can_send_request_and_receive_response(string method, Uri uri)
        {
            var httpMethod = new HttpMethod(method);

            var options = new[]
            {
                new HttpMessageOptions
                {
                    RequestUri = uri,
                    HttpMethod = httpMethod,
                    HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                }
            };
            var sut = CreateSystemUnderTest(options);

            await sut.SendAsync(httpMethod, uri.ToString());
        }

        [Test]
        [InlineAutoData("GET", HttpStatusCode.NotFound)]
        [InlineAutoData("POST", HttpStatusCode.NotFound)]
        [InlineAutoData("PUT", HttpStatusCode.NotFound)]
        [InlineAutoData("DELETE", HttpStatusCode.NotFound)]
        public void SendAsync_throws_HttpException_if_nonSuccessful_response(string method, HttpStatusCode statusCode, Uri uri)
        {
            var httpMethod = new HttpMethod(method);

            var options = new[]
            {
                new HttpMessageOptions
                {
                    RequestUri = uri,
                    HttpMethod = httpMethod,
                    HttpResponseMessage = new HttpResponseMessage(statusCode)
                }
            };

            var sut = CreateSystemUnderTest(options);

            Assert.ThrowsAsync<HttpException>(() => sut.SendAsync(httpMethod, uri.ToString()));
        }

        public class Request
        {
            public int IntValue { get; set; }

            public string StringValue { get; set; }

            public DateTimeOffset DateTimeOffsetValue { get; set; }
        }

        public class Response : IEquatable<Response>
        {
            public int IntValue { get; set; }

            public string StringValue { get; set; }

            public DateTimeOffset DateTimeOffsetValue { get; set; }

            bool IEquatable<Response>.Equals(Response other)
            {
                if (other == null)
                    return false;

                if (IntValue != other.IntValue)
                    return false;

                if (StringValue != other.StringValue)
                    return false;

                if (DateTimeOffsetValue != other.DateTimeOffsetValue)
                    return false;

                return true;
            }
        }

    }
}