using System;
using System.Threading.Tasks;
using ArtAuction.Infrastructure.IntegrationTests.DataAttributes;
using ArtAuction.Infrastructure.Persistence.Repositories;
using ArtAuction.Tests.Base;
using ArtAuction.Tests.Base.Attributes;
using FluentAssertions;
using Xunit;

namespace ArtAuction.Infrastructure.IntegrationTests.Repositories
{
    public class AuctionRepositoryTests
    {
        private const string AuctionId = "{499C0593-F1DC-4254-A449-5CC4E134366C}";
        private const int AuctionNumber = 14698847;
        private const string LotId = "{C60D7A78-E817-4B00-8AF3-07326E329CC0}";

        private const string SellerId = "{3EDF125C-64BC-4487-93DE-85E17E902FA7}";
        private const string UserId = "{EB983C2A-BFE6-4180-9062-9F6D9ECC7BB9}";

        private const string DateTime1 = "2022-01-01 12:30:30";
        private const string DateTime2 = "2022-01-02 11:45:20";

        [Fact, MockAutoData]
        [UseUser(UserId = SellerId)]
        [UseUser(UserId = UserId)]
        [UseAuction(AuctionId = AuctionId, AuctionNumber = AuctionNumber, LotId = LotId, SellerId = SellerId)]
        [UseBid(AuctionId = AuctionId, UserId = UserId, DateTime = DateTime1)]
        [UseBid(AuctionId = AuctionId, UserId = UserId, DateTime = DateTime2)]
        [UseMessage(AuctionId = AuctionId, UserId = SellerId, DateTime = DateTime1)]
        [UseMessage(AuctionId = AuctionId, UserId = UserId, DateTime = DateTime2)]
        public async Task repository_gets_auction_correctly()
        {
            // Arrange
            var sut = new AuctionRepository(TestConfiguration.Get());
            
            // Act
            var result = await sut.GetAuctionAsync(AuctionNumber);

            // Assert
            result.Should().NotBeNull();
            result.AuctionId.Should().Be(new Guid(AuctionId));
            result.AuctionNumber.Should().Be(AuctionNumber);
            result.SellerId.Should().Be(new Guid(SellerId));

            result.Lot.Should().NotBeNull();
            result.Lot.LotId.Should().Be(new Guid(LotId));
            result.LotId.Should().Be(new Guid(LotId));

            result.Bids.Should().NotBeNullOrEmpty();
            result.Bids.Should().HaveCount(2);
            result.Bids.Should().OnlyContain(b => b.AuctionId == new Guid(AuctionId));
            
            result.Messages.Should().NotBeNullOrEmpty();
            result.Messages.Should().HaveCount(2);
            result.Messages.Should().OnlyContain(b => b.AuctionId == new Guid(AuctionId));
        }
    }
}