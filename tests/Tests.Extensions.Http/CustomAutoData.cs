using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Kralizek.Extensions.Http;
using Newtonsoft.Json;

namespace Tests
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base(FixtureHelper.CreateFixture) { }
    }

    public class InlineCustomAutoDataAttribute : InlineAutoDataAttribute
    {
        public InlineCustomAutoDataAttribute(params object[] arguments) : base(FixtureHelper.CreateFixture, arguments) { }
    }

    public static class FixtureHelper
    {
        public static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });

            fixture.AddMockHttp();

            fixture.Inject(new JsonSerializerSettings());

            fixture.Inject(new HttpRestClientOptions());

            return fixture;
        }
    }
}
