using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Tests.Extensions.Http
{
    [TestFixture]
    public class HttpRestClientTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HttpRestClient).GetConstructors());
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public async Task SendAsync_can_send_request_with_body_and_receive_response_with_body(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Request request, Response response)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithJsonContent(request)
                    .Respond(HttpStatusCode.OK, JsonContent.FromObject(response));

            var actualResponse = await sut.SendAsync<Request, Response>(httpMethod, uri.ToString(), request);

            Assert.That(actualResponse, Is.EqualTo(response));
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_throws_HttpException_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Request request, Response response)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithJsonContent(request)
                    .Respond(HttpStatusCode.NotFound, JsonContent.FromObject(response));

            Assert.That(() => sut.SendAsync<Request, Response>(httpMethod, uri.ToString(), request), Throws.TypeOf<HttpException>());
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_can_send_request_with_body_and_receive_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Request request)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithJsonContent(request)
                    .Respond(HttpStatusCode.OK);

            Assert.That(() => sut.SendAsync(httpMethod, uri.ToString(), request), Throws.Nothing);
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_throws_HttpException_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Request request)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithJsonContent(request)
                    .Respond(HttpStatusCode.NotFound);

            Assert.That(() => sut.SendAsync(httpMethod, uri.ToString(), request), Throws.TypeOf<HttpException>());
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public async Task SendAsync_can_send_request_and_receive_response_with_body(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Response response)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .Respond(HttpStatusCode.OK, JsonContent.FromObject(response));

            var actualResponse = await sut.SendAsync<Response>(httpMethod, uri.ToString());

            Assert.That(actualResponse, Is.EqualTo(response));
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_throws_HttpException_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Response response)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .Respond(HttpStatusCode.NotFound, JsonContent.FromObject(response));

            Assert.That(() => sut.SendAsync<Response>(httpMethod, uri.ToString()), Throws.TypeOf<HttpException>());
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_can_send_request_and_receive_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .Respond(HttpStatusCode.OK);

            Assert.That(() => sut.SendAsync(httpMethod, uri.ToString()), Throws.Nothing);
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_throws_HttpException_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .Respond(HttpStatusCode.NotFound);

            Assert.That(() => sut.SendAsync<Response>(httpMethod, uri.ToString()), Throws.TypeOf<HttpException>());
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public async Task HttpClientName_is_used_when_specified_in_options(string method, [Frozen] MockHttpMessageHandler handler, [Frozen] IHttpClientFactory httpClientFactory, [Frozen] HttpRestClientOptions options, HttpRestClient sut, Uri uri, string httpClientName)
        {
            var httpMethod = new HttpMethod(method);

            options.HttpClientName = httpClientName;

            handler.When(httpMethod, uri.ToString())
                    .Respond(HttpStatusCode.OK);

            await sut.SendAsync(httpMethod, uri.ToString());

            Mock.Get(httpClientFactory).Verify(p => p.CreateClient(httpClientName), Times.AtLeastOnce());
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