using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Infrastructure.IntegrationTests.DataAttributes;
using ArtAuction.Infrastructure.Persistence;
using ArtAuction.Infrastructure.Persistence.Repositories;
using ArtAuction.Tests.Base;
using ArtAuction.Tests.Base.Attributes;
using Dapper;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ArtAuction.Infrastructure.IntegrationTests.Repositories
{
    public class AuctionRepositoryTests
    {
        private const string AuctionId = "{499C0593-F1DC-4254-A449-5CC4E134366C}";
        private const int AuctionNumber = 14698847;
        private const string LotId = "{C60D7A78-E817-4B00-8AF3-07326E329CC0}";
        private const string CategoryName = "Some Test CategoryName";
        
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

        [Theory, MockAutoData]
        [UseCategory(CategoryName = CategoryName)]
        [UseUser(UserId = SellerId)]
        [RemoveAuctionAfter(SellerId)]
        public async Task repository_adds_auction_correctly(
            Auction auction
        )
        {
            // Arrange
            var sut = new AuctionRepository(TestConfiguration.Get());
            
            auction.SellerId = new Guid(SellerId);
            auction.Lot.Category.Name = CategoryName;

            // Act
            await sut.AddAuctionAsync(auction);

            // Assert
            var result = await GetAuctionAsync(SellerId);
            
            result.Should().NotBeNull();
            result.SellerId.Should().Be(new Guid(SellerId));
            
            result.Lot.Should().NotBeNull();
            result.Lot.Category.Name.Should().Be(CategoryName);
        }

        [Theory, MockAutoData]
        [UseUser(UserId = SellerId)]
        [UseUser(UserId = UserId)]
        [UseAuction(AuctionId = AuctionId, AuctionNumber = AuctionNumber, SellerId = SellerId)]
        [RemoveBidsAfter(AuctionId)]
        public async Task repository_adds_bid_correctly(
            Bid bid
        )
        {
            // Arrange
            var sut = new AuctionRepository(TestConfiguration.Get());

            bid.AuctionId = new Guid(AuctionId);
            bid.UserId = new Guid(UserId);

            // Act
            await sut.AddBidAsync(bid);

            // Assert
            var result = (await GetBids(new Guid(AuctionId))).ToArray();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.First().AuctionId.Should().Be(AuctionId);
            result.First().UserId.Should().Be(UserId);
        }

        [Theory, MockAutoData]
        [UseUser(UserId = SellerId)]
        [UseUser(UserId = UserId)]
        [UseAuction(AuctionId = AuctionId, AuctionNumber = AuctionNumber, SellerId = SellerId)]
        [RemoveMessagesAfter(AuctionId)]
        public async Task repository_adds_messages_correctly(
            Message message
        )
        {
            // Arrange
            var sut = new AuctionRepository(TestConfiguration.Get());

            message.AuctionId = new Guid(AuctionId);
            message.UserId = new Guid(UserId);

            // Act
            await sut.AddMessageAsync(message);

            // Assert
            var result = (await GetMessages(new Guid(AuctionId))).ToArray();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.First().AuctionId.Should().Be(AuctionId);
            result.First().UserId.Should().Be(UserId);
        }

        [Fact, MockAutoData]
        [UseUser(UserId = SellerId)]
        [UseUser(UserId = UserId)]
        [UseCategory]
        [UseAuction(AuctionId = AuctionId, AuctionNumber = AuctionNumber, SellerId = SellerId)]
        public async Task repository_gets_categories_correctly()
        {
            // Arrange
            var sut = new AuctionRepository(TestConfiguration.Get());
            
            // Act
            var result = await sut.GetCategories();

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        #region Private methods

        private async Task<Auction> GetAuctionAsync(string sellerId)
        {
            Auction auction;

            var query = @"
                SELECT 
	                 [auction_id] AS AuctionId
	                ,[auction_number] AS AuctionNumber
                    ,[lot_id] AS LotId
                    ,[seller_id] AS SellerId
                    ,[start_billing_date] AS StartBillingDate
                    ,[end_billing_date] AS EndBillingDate
                    ,[start_price] AS StartPrice
                    ,[current_price] AS CurrentPrice
                    ,[full_price] AS FullPrice
                    ,[bid_step] AS BidStep
                    ,[is_closed] AS IsClosed
                    ,[customer_id] AS CustomerId
                FROM [dbo].[auction]
                WHERE 
                    [seller_id] = @sellerId";

            await using (var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                await connection.OpenAsync();
                await using (var transaction = await connection.BeginTransactionAsync())
                {
                    auction = await connection.QueryFirstOrDefaultAsync<Auction>(query, new
                    {
                        sellerId
                    }, transaction);

                    if (auction != null)
                    {
                        auction.Lot = await GetLot(auction.LotId, connection, transaction);
                    }

                    await transaction.CommitAsync();
                }
            }

            return auction;
        }

        private async Task<Lot> GetLot(Guid lotId, IDbConnection connection, IDbTransaction transaction)
        {
            var query = @"
                SELECT
	                 [lot_id] AS LotId
                    ,[category_id] AS CategoryId
                    ,[name]
                    ,[painting_date] AS PaintingDate
                    ,[photo]
                    ,[description]
                FROM [dbo].[lot]
                WHERE
	                [lot_id] = @lotId";

            var lot = await connection.QueryFirstOrDefaultAsync<Lot>(query, new
            {
                lotId
            }, transaction);

            if (lot != null)
            {
                lot.Category = await GetCategory(lot.CategoryId, connection, transaction);
            }

            return lot;
        }

        private async Task<Category> GetCategory(Guid categoryId, IDbConnection connection, IDbTransaction transaction)
        {
            var query = @"
                SELECT 
	                 [category_id] AS CategoryId
                    ,[name]
                FROM [dbo].[category]
                WHERE 
	                [category_id] = @categoryId";

            return await connection.QueryFirstOrDefaultAsync<Category>(query, new
            {
                categoryId
            }, transaction);
        }

        private async Task<IEnumerable<Bid>> GetBids(Guid auctionId)
        {
            var query = @"
                SELECT 
	                 [bid_id] AS BidId
                    ,[user_id] AS UserId
                    ,[auction_id] AS AuctionId
                    ,[date_time] AS DateTime
                    ,[sum]
                FROM [dbo].[bid]
                WHERE 
	                [auction_id] = @auctionId";

            await using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            return await connection.QueryAsync<Bid>(query, new
            {
                auctionId
            });
        }

        private async Task<IEnumerable<Message>> GetMessages(Guid auctionId)
        {
            var query = @"
                SELECT
	                 [message_id] AS MessageId
                    ,[user_id] AS UserId
                    ,[auction_id] AS AuctionId
                    ,[date_time] AS DateTime
                    ,[message_text] AS MessageText
                    ,[is_admin] AS IsAdmin
                FROM [dbo].[message]
                WHERE 
	                [auction_id] = @auctionId";

            await using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            return await connection.QueryAsync<Message>(query, new
            {
                auctionId
            });
        }

        #endregion
    }
}