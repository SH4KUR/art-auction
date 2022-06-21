using System.Threading.Tasks;
using ArtAuction.Tests.Base.Attributes;
using FluentAssertions;
using Xunit;

namespace ArtAuction.Infrastructure.IntegrationTests.Repositories
{
    public class AccountRepositoryTests
    {
        [Fact, MockAutoData]
        public async Task repository_gets_account_correctly()
        {
            true.Should().BeTrue();
        }

        [Fact, MockAutoData]
        public async Task repository_adds_operation_correctly()
        {
            true.Should().BeTrue();
        }

        [Fact, MockAutoData]
        public async Task repository_adds_vip_correctly()
        {
            true.Should().BeTrue();
        }

        [Fact, MockAutoData]
        public async Task repository_updates_account_correctly()
        {
            true.Should().BeTrue();
        }
    }
}