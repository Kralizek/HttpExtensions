using System.Net.Http;
using System.Threading.Tasks;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;

namespace Tests.Extensions.Http
{
    [TestFixture]
    public class HttpRestClientExtensionsTests
    {
        [Test, CustomAutoData]
        public void GetAsync_throws_if_client_is_null(string path)
        {
            Assert.That(() => HttpRestClientExtensions.GetAsync(null, path), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void GetAsync_throws_if_client_is_null(string path, Request request)
        {
            Assert.That(() => HttpRestClientExtensions.GetAsync(null, path, request), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void GetAsync_throws_if_client_is_null(string path, Response _)
        {
            Assert.That(() => HttpRestClientExtensions.GetAsync<Response>(null, path), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void GetAsync_throws_if_client_is_null(string path, Request request, Response _)
        {
            Assert.That(() => HttpRestClientExtensions.GetAsync<Request, Response>(null, path, request), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public async Task GetAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, IQueryString queryString)
        {
            await client.GetAsync(path, queryString);

            Mock.Get(client).Verify(p => p.SendAsync(HttpMethod.Get, path, queryString));
        }

        [Test, CustomAutoData]
        public async Task GetAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, Request request, IQueryString queryString)
        {
            await client.GetAsync(path, request, queryString);

            Mock.Get(client).Verify(p => p.SendAsync(HttpMethod.Get, path, request, queryString));
        }

        [Test, CustomAutoData]
        public async Task GetAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, IQueryString queryString, Response _)
        {
            _ = await client.GetAsync<Response>(path, queryString);

            Mock.Get(client).Verify(p => p.SendAsync<Response>(HttpMethod.Get, path, queryString));
        }

        [Test, CustomAutoData]
        public async Task GetAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, Request request, IQueryString queryString, Response _)
        {
            _ = await client.GetAsync<Request, Response>(path, request, queryString);

            Mock.Get(client).Verify(p => p.SendAsync<Request, Response>(HttpMethod.Get, path, request, queryString));
        } 

        [Test, CustomAutoData]
        public void PostAsync_throws_if_client_is_null(string path)
        {
            Assert.That(() => HttpRestClientExtensions.PostAsync(null, path), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void PostAsync_throws_if_client_is_null(string path, Request request)
        {
            Assert.That(() => HttpRestClientExtensions.PostAsync(null, path, request), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void PostAsync_throws_if_client_is_null(string path, Response _)
        {
            Assert.That(() => HttpRestClientExtensions.PostAsync<Response>(null, path), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void PostAsync_throws_if_client_is_null(string path, Request request, Response _)
        {
            Assert.That(() => HttpRestClientExtensions.PostAsync<Request, Response>(null, path, request), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public async Task PostAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, IQueryString queryString)
        {
            await client.PostAsync(path, queryString);

            Mock.Get(client).Verify(p => p.SendAsync(HttpMethod.Post, path, queryString));
        }

        [Test, CustomAutoData]
        public async Task PostAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, Request request, IQueryString queryString)
        {
            await client.PostAsync(path, request, queryString);

            Mock.Get(client).Verify(p => p.SendAsync(HttpMethod.Post, path, request, queryString));
        }

        [Test, CustomAutoData]
        public async Task PostAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, IQueryString queryString, Response _)
        {
            _ = await client.PostAsync<Response>(path, queryString);

            Mock.Get(client).Verify(p => p.SendAsync<Response>(HttpMethod.Post, path, queryString));
        }

        [Test, CustomAutoData]
        public async Task PostAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, Request request, IQueryString queryString, Response _)
        {
            _ = await client.PostAsync<Request, Response>(path, request, queryString);

            Mock.Get(client).Verify(p => p.SendAsync<Request, Response>(HttpMethod.Post, path, request, queryString));
        } 

        [Test, CustomAutoData]
        public void PutAsync_throws_if_client_is_null(string path)
        {
            Assert.That(() => HttpRestClientExtensions.PutAsync(null, path), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void PutAsync_throws_if_client_is_null(string path, Request request)
        {
            Assert.That(() => HttpRestClientExtensions.PutAsync(null, path, request), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void PutAsync_throws_if_client_is_null(string path, Response _)
        {
            Assert.That(() => HttpRestClientExtensions.PutAsync<Response>(null, path), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void PutAsync_throws_if_client_is_null(string path, Request request, Response _)
        {
            Assert.That(() => HttpRestClientExtensions.PutAsync<Request, Response>(null, path, request), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public async Task PutAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, IQueryString queryString)
        {
            await client.PutAsync(path, queryString);

            Mock.Get(client).Verify(p => p.SendAsync(HttpMethod.Put, path, queryString));
        }

        [Test, CustomAutoData]
        public async Task PutAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, Request request, IQueryString queryString)
        {
            await client.PutAsync(path, request, queryString);

            Mock.Get(client).Verify(p => p.SendAsync(HttpMethod.Put, path, request, queryString));
        }

        [Test, CustomAutoData]
        public async Task PutAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, IQueryString queryString, Response _)
        {
            _ = await client.PutAsync<Response>(path, queryString);

            Mock.Get(client).Verify(p => p.SendAsync<Response>(HttpMethod.Put, path, queryString));
        }

        [Test, CustomAutoData]
        public async Task PutAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, Request request, IQueryString queryString, Response _)
        {
            _ = await client.PutAsync<Request, Response>(path, request, queryString);

            Mock.Get(client).Verify(p => p.SendAsync<Request, Response>(HttpMethod.Put, path, request, queryString));
        }

        [Test, CustomAutoData]
        public void DeleteAsync_throws_if_client_is_null(string path)
        {
            Assert.That(() => HttpRestClientExtensions.DeleteAsync(null, path), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void DeleteAsync_throws_if_client_is_null(string path, Request request)
        {
            Assert.That(() => HttpRestClientExtensions.DeleteAsync(null, path, request), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void DeleteAsync_throws_if_client_is_null(string path, Response _)
        {
            Assert.That(() => HttpRestClientExtensions.DeleteAsync<Response>(null, path), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public void DeleteAsync_throws_if_client_is_null(string path, Request request, Response _)
        {
            Assert.That(() => HttpRestClientExtensions.DeleteAsync<Request, Response>(null, path, request), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public async Task DeleteAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, IQueryString queryString)
        {
            await client.DeleteAsync(path, queryString);

            Mock.Get(client).Verify(p => p.SendAsync(HttpMethod.Delete, path, queryString));
        }

        [Test, CustomAutoData]
        public async Task DeleteAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, Request request, IQueryString queryString)
        {
            await client.DeleteAsync(path, request, queryString);

            Mock.Get(client).Verify(p => p.SendAsync(HttpMethod.Delete, path, request, queryString));
        }

        [Test, CustomAutoData]
        public async Task DeleteAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, IQueryString queryString, Response _)
        {
            _ = await client.DeleteAsync<Response>(path, queryString);

            Mock.Get(client).Verify(p => p.SendAsync<Response>(HttpMethod.Delete, path, queryString));
        }

        [Test, CustomAutoData]
        public async Task DeleteAsync_forwards_to_IHttpRestClient(IHttpRestClient client, string path, Request request, IQueryString queryString, Response _)
        {
            _ = await client.DeleteAsync<Request, Response>(path, request, queryString);

            Mock.Get(client).Verify(p => p.SendAsync<Request, Response>(HttpMethod.Delete, path, request, queryString));
        }
    }
}
