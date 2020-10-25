# Kralizek's HTTP Extensions

## Overview

This repository contains a set of extensions to ease working with HTTP requests and response in .NET.

### Kralizek.Extensions.Http

This package offers basic HTTP utilities.

#### Query string

The `HttpQueryStringBuilder` class helps creating a valid query string using the same methodology of the `StringBuilder`.

```csharp
var builder = new HttpQueryStringBuilder();

builder.Add("foo", "bar");
builder.Add("hello", "world");
builder.Add("asd", "lol");

string query = builder.BuildQuery();

Console.WriteLine(query);  // asd=lol&foo=bar&hello=world
```

The `BuildQuery` method offers the possibility of collating items with the same key with a separator.

```csharp
var builder = new HttpQueryStringBuilder();

builder.Add("fields", "firstName");
builder.Add("fields", "lastName");

var query = builder.BuildQuery(collateKeysBy: ",");

Console.WriteLine(query);  // fields=firstName,lastName
```

### Kralizek.Extensions.Http.Json

This package offers support for JSON payloads.

#### JSON payloads

The `JsonContent` class inherits from `System.Net.Http.HttpContent`, therefore it can be used to attach JSON payloads to an HTTP request.

The static factory method `FromObject<T>` accepts any object and uses _Newtonsoft.Json_ to serialize it into a JSON object.

```csharp
var person = new Person
{
    FirstName = "John",
    LastName = "Doe"
};

using var request = new HttpRequestMessage(HttpMethod.Post, "http://localtest.me:8080") 
{
    Content = JsonContent.FromObject(person)
};

using var response = await http.SendAsync(request);
```

#### HTTP REST Client

The `HttpRestClient` is an opinionated wrapper around the `HttpClient`.

Here are some advantages of using the `HttpRestClient`:

- it delegates the `HttpClient` instance management to `IHttpClientFactory`,
- offers methods to send HTTP requests with and without payload expecting or not a payload in the response,
- it embeds logging using the `Microsoft.Extensions.Logging` framework,
- it automatically handles serialization and deserialization to and from JSON payloads,
- it integrates nicely with the `Microsoft.Extensions.DependencyInjection` framework.

Here is a sample of its usage.

```csharp
var client = services.GetRequiredServices<HttpRestClient>();

var person = await client.SendAsync<Person>(HttpMethod.Get, "/people/1", query);
```

In the snippet above, the call to the `SendAsync` method takes care of:

- getting an instance of `HttpClient` from the local `IHttpClientFactory`,
- assemble the HTTP request using the specified HTTP method, the path and the query from the previous snippet,
- log the HTTP request,
- send the HTTP request,
- receive the HTTP response,
- log the HTTP response,
- validate the result of the request via the status code of the HTTP response,
- deserialize the payload into an instance of the `Person` class,
- dispose all the resources that need disposing (`HttpRequestMessage` and `HttpResponseMessage`),

## Versioning

This library follows [Semantic Versioning 2.0.0](http://semver.org/spec/v2.0.0.html) for the public releases (published to the [nuget.org](https://www.nuget.org/)).


## How to build

This project uses [Cake](https://cakebuild.net/) as a build engine. You will also need the [.NET Core SDK 3.1.401](https://dotnet.microsoft.com/).

If you would like to build this project locally, just execute the `build.cake` script.

You can do it by using the .NET tool created by CAKE authors and use it to execute the build script.

```powershell
dotnet tool restore
dotnet cake
```
