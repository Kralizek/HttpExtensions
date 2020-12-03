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
        public async Task SendAsync_with_Request_and_Response_can_send_request_with_body_and_receive_response_with_body(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Request request, Response response)
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
        public void SendAsync_with_Request_and_Response_throws_HttpException_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Request request)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithJsonContent(request)
                    .Respond(HttpStatusCode.NotFound);

            Assert.That(() => sut.SendAsync<Request, Response>(httpMethod, uri.ToString(), request), Throws.TypeOf<HttpException>());
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_with_Request_and_Response_attaches_error_payload_to_exception_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Request request, string errorPayload)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithJsonContent(request)
                    .Respond(HttpStatusCode.NotFound, "text/plain", errorPayload);

            Assert.That(() => sut.SendAsync<Request, Response>(httpMethod, uri.ToString(), request), Throws.TypeOf<HttpException>().And.Property(nameof(HttpException.Payload)).EqualTo(errorPayload));
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public async Task SendAsync_with_HttpContent_and_Response_can_send_request_with_body_and_receive_response_with_body(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, string content, Response response)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithContent(content)
                    .Respond(HttpStatusCode.OK, JsonContent.FromObject(response));

            var httpContent = new StringContent(content);

            var actualResponse = await sut.SendAsync<Response>(httpMethod, uri.ToString(), httpContent);

            Assert.That(actualResponse, Is.EqualTo(response));
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_with_HttpContent_and_Response_throws_HttpException_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, string content)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithContent(content)
                    .Respond(HttpStatusCode.NotFound);

            var httpContent = new StringContent(content);

            Assert.That(() => sut.SendAsync<Response>(httpMethod, uri.ToString(), httpContent), Throws.TypeOf<HttpException>());
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_with_HttpContent_and_Response_attaches_error_payload_to_exception_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, string content, string errorPayload)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithContent(content)
                    .Respond(HttpStatusCode.NotFound, "text/plain", errorPayload);

            var httpContent = new StringContent(content);

            Assert.That(() => sut.SendAsync<Response>(httpMethod, uri.ToString(), httpContent), Throws.TypeOf<HttpException>().And.Property(nameof(HttpException.Payload)).EqualTo(errorPayload));
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_with_Request_can_send_request_with_body_and_receive_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Request request)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithJsonContent(request)
                    .Respond(HttpStatusCode.OK);

            Assert.That(() => sut.SendAsync<Request>(httpMethod, uri.ToString(), request), Throws.Nothing);
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_with_Request_throws_HttpException_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Request request)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithJsonContent(request)
                    .Respond(HttpStatusCode.NotFound);

            Assert.That(() => sut.SendAsync<Request>(httpMethod, uri.ToString(), request), Throws.TypeOf<HttpException>());
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_with_Request_attaches_error_payload_to_exception_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Request request, string errorPayload)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithJsonContent(request)
                    .Respond(HttpStatusCode.NotFound, "text/plain", errorPayload);

            Assert.That(() => sut.SendAsync<Request>(httpMethod, uri.ToString(), request), Throws.TypeOf<HttpException>().And.Property(nameof(HttpException.Payload)).EqualTo(errorPayload));
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_with_HttpContent_can_send_request_with_body_and_receive_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, string content)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithContent(content)
                    .Respond(HttpStatusCode.OK);

            var httpContent = new StringContent(content);

            Assert.That(() => sut.SendAsync(httpMethod, uri.ToString(), httpContent), Throws.Nothing);
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_with_HttpContent_throws_HttpException_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, string content)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithContent(content)
                    .Respond(HttpStatusCode.NotFound);

            var httpContent = new StringContent(content);

            Assert.That(() => sut.SendAsync(httpMethod, uri.ToString(), httpContent), Throws.TypeOf<HttpException>());
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_with_HttpContent_attaches_error_payload_to_exception_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, string content, string errorPayload)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .WithContent(content)
                    .Respond(HttpStatusCode.NotFound, "text/plain", errorPayload);

            var httpContent = new StringContent(content);

            Assert.That(() => sut.SendAsync(httpMethod, uri.ToString(), httpContent), Throws.TypeOf<HttpException>().And.Property(nameof(HttpException.Payload)).EqualTo(errorPayload));
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public async Task SendAsync_with_Response_can_send_request_and_receive_response_with_body(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, Response response)
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
        public void SendAsync_with_Response_throws_HttpException_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri)
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
        public void SendAsync_with_Response_attaches_error_payload_to_exception_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, string errorPayload)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .Respond(HttpStatusCode.NotFound, "text/plain", errorPayload);

            Assert.That(() => sut.SendAsync<Response>(httpMethod, uri.ToString()), Throws.TypeOf<HttpException>().And.Property(nameof(HttpException.Payload)).EqualTo(errorPayload));
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

            Assert.That(() => sut.SendAsync(httpMethod, uri.ToString()), Throws.TypeOf<HttpException>());
        }

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void SendAsync_attaches_error_payload_to_exception_if_nonSuccessful_response(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri, string errorPayload)
        {
            var httpMethod = new HttpMethod(method);

            handler.When(httpMethod, uri.ToString())
                    .Respond(HttpStatusCode.NotFound, "text/plain", errorPayload);

            Assert.That(() => sut.SendAsync(httpMethod, uri.ToString()), Throws.TypeOf<HttpException>().And.Property(nameof(HttpException.Payload)).EqualTo(errorPayload));
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

        [Test]
        [InlineCustomAutoData("GET")]
        [InlineCustomAutoData("POST")]
        [InlineCustomAutoData("PUT")]
        [InlineCustomAutoData("DELETE")]
        public void HttpClient_returns_an_instance_of_HttpClient_configured(string method, [Frozen] MockHttpMessageHandler handler, HttpRestClient sut, Uri uri)
        {
            var httpMethod = new HttpMethod(method);

            using var httpRequest = new HttpRequestMessage(httpMethod, uri);

            handler.When(httpMethod, uri.ToString())
                    .Respond(HttpStatusCode.OK);

            var httpClient = sut.HttpClient;

            Assert.That(() => httpClient.SendAsync(httpRequest), Throws.Nothing);
        }
    }
}
