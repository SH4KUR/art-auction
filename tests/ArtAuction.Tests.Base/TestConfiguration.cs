using Microsoft.Extensions.Configuration;

namespace ArtAuction.Tests.Base
{
    public static class TestConfiguration
    {
        public static IConfiguration Get()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.tests.json")
                .Build();
        }
    }
}