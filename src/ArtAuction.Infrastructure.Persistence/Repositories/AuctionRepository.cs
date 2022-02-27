using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Core.Domain.Enums;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.Infrastructure.Persistence.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly IConfiguration _configuration;

        public AuctionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Auction> GetAuctionAsync(int auctionNumber)
        {
            Auction auction;
            
            var query = @"
                SELECT 
	                 [auction_id] AS AuctionId
	                ,[auction_number] AS AuctionNumber
                    ,[lot_id] AS LotId
                    ,[seller_id] AS SellerId
                    ,[creation_datetime] AS CreationDateTime
                    ,[start_billing_datetime] AS StartBillingDateTime
                    ,[end_billing_datetime] AS EndBillingDateTime
                    ,[start_price] AS StartPrice
                    ,[current_price] AS CurrentPrice
                    ,[full_price] AS FullPrice
                    ,[bid_step] AS BidStep
                    ,[is_closed] AS IsClosed
                    ,[customer_id] AS CustomerId
                FROM [dbo].[auction]
                WHERE 
                    [auction_number] = @auctionNumber";

            await using (var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                await connection.OpenAsync();
                await using (var transaction = await connection.BeginTransactionAsync())
                {
                    auction = await connection.QueryFirstOrDefaultAsync<Auction>(query, new
                    {
                        auctionNumber
                    }, transaction);
                    
                    if (auction != null)
                    {
                        auction.Lot = await GetLot(auction.LotId, connection, transaction);
                        auction.Bids = await GetBids(auction.AuctionId, connection, transaction);
                        auction.Messages = await GetMessages(auction.AuctionId, connection, transaction);
                    }
                    
                    await transaction.CommitAsync();
                }
            }

            return auction;
        }

        public async Task<IEnumerable<Auction>> GetAuctionsAsync(
            SortingRule sort, 
            IEnumerable<string> filterCategories, 
            decimal? minCurrentPrice, 
            decimal? maxCurrentPrice,
            int pageNumber = 1,
            int rowsOnPage = 10,
            bool isClosed = false)
        {
            var categoriesFilterSql = GetCategoriesFilterSql(filterCategories.ToArray());
            var currentPriceFilterSql = GetCurrentPriceFilterSql(minCurrentPrice, maxCurrentPrice);
            var auctionSortSql = GetAuctionSortSql(sort);

            var query = $@"
                SELECT 
	                 [auction_id] AS AuctionId
	                ,[auction_number] AS AuctionNumber
                    ,a.[lot_id] AS LotId
                    ,[seller_id] AS SellerId
                    ,[creation_datetime] AS CreationDateTime
                    ,[start_billing_datetime] AS StartBillingDateTime
                    ,[end_billing_datetime] AS EndBillingDateTime
                    ,[start_price] AS StartPrice
                    ,[current_price] AS CurrentPrice
                    ,[full_price] AS FullPrice
                    ,[bid_step] AS BidStep
                    ,[is_closed] AS IsClosed
                    ,[customer_id] AS CustomerId
                FROM [dbo].[auction] AS a 
	                INNER JOIN [dbo].[lot] AS l ON a.[lot_id] = l.[lot_id]
	                INNER JOIN [dbo].[category] AS c ON l.[category_id] = c.[category_id]
                WHERE 
                    a.[is_closed] = {(isClosed ? "1" : "0")}
                    {categoriesFilterSql}
                    {currentPriceFilterSql}
                ORDER BY {auctionSortSql}
                OFFSET {(pageNumber - 1) * rowsOnPage} ROWS
                FETCH NEXT {rowsOnPage} ROWS ONLY
            ";

            List<Auction> auctions;
            await using (var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                await connection.OpenAsync();
                await using (var transaction = await connection.BeginTransactionAsync())
                {
                    auctions = (List<Auction>) await connection.QueryAsync<Auction>(query, transaction: transaction);

                    if (auctions.Any())
                    {
                        foreach (var auction in auctions)
                        {
                            auction.Lot = await GetLot(auction.LotId, connection, transaction);
                            auction.Bids = await GetBids(auction.AuctionId, connection, transaction);
                            auction.Messages = await GetMessages(auction.AuctionId, connection, transaction);
                        }
                    }

                    await transaction.CommitAsync();
                }
            }

            return auctions;
        }

        private string GetCategoriesFilterSql(string[] filterCategories)
        {
            return filterCategories.Any()
                ? $", c.[name] IN ('{string.Join("', '", filterCategories)}')"
                : string.Empty;
        }

        private string GetCurrentPriceFilterSql(decimal? minCurrentPrice, decimal? maxCurrentPrice)
        {
            if (maxCurrentPrice == null && minCurrentPrice == null)
            {
                return string.Empty;
            }

            var sqlFilter = new StringBuilder();

            if (maxCurrentPrice != null)
            {
                sqlFilter.AppendLine($", a.[current_price] <= {maxCurrentPrice}");
            }
            if (minCurrentPrice != null)
            {
                sqlFilter.AppendLine($", a.[current_price] >= {minCurrentPrice}");
            }

            return sqlFilter.ToString();
        }

        private string GetAuctionSortSql(SortingRule sort)
        {
            return sort switch
            {
                SortingRule.Default => "a.[datetime_create] DESC",      // Default sorting by "Newest Created"
                SortingRule.CurrentPriceAsc => "a.[current_price]",
                SortingRule.CurrentPriceDesc => "a.[current_price] DESC",
                SortingRule.DateTimeAuctionCreateAsc => "a.[datetime_create]",
                SortingRule.DateTimeAuctionCreateDesc => "a.[datetime_create] DESC",
                SortingRule.DateTimeAuctionEndAsc => "a.[end_billing_date]",
                SortingRule.DateTimeAuctionEndDesc => "a.[end_billing_date] DESC",
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, "Sorting rule error!")
            };
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

        private async Task<IEnumerable<Bid>> GetBids(Guid auctionId, IDbConnection connection, IDbTransaction transaction)
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

            return await connection.QueryAsync<Bid>(query, new
            {
                auctionId
            }, transaction);
        }

        private async Task<IEnumerable<Message>> GetMessages(Guid auctionId, IDbConnection connection, IDbTransaction transaction)
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

            return await connection.QueryAsync<Message>(query, new
            {
                auctionId
            }, transaction);
        }

        public async Task AddAuctionAsync(Auction auction)
        {
            var query = @"
                DECLARE @InsertedLot TABLE ([lot_id] UNIQUEIDENTIFIER)

                INSERT INTO [dbo].[lot] (
                     [category_id]
                    ,[name]
                    ,[painting_date]
                    ,[photo]
                    ,[description]
                )
                OUTPUT INSERTED.[lot_id] INTO @InsertedLot
                VALUES (
                     (SELECT TOP 1 [category_id] FROM [dbo].[category] WHERE [name] = @CategoryName)
                    ,@LotName
                    ,@PaintingDate
                    ,@Photo
                    ,@Description
                )

                INSERT INTO [dbo].[auction] (
                     [lot_id]
                    ,[seller_id]
                    ,[creation_datetime]
                    ,[start_billing_datetime]
                    ,[end_billing_datetime]
                    ,[start_price]
                    ,[current_price]
                    ,[full_price]
                    ,[bid_step]
                    ,[is_closed]
                    ,[customer_id]
                )
                VALUES (
                     (SELECT TOP 1 [lot_id] FROM @InsertedLot)
	                ,@SellerId
                    ,GETDATE()
                    ,@StartBillingDateTime
	                ,@EndBillingDateTime
	                ,@StartPrice
	                ,@CurrentPrice
	                ,@FullPrice
	                ,@BidStep
	                ,0
                    ,NULL
                )";

            await using (var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                await connection.OpenAsync();
                await using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        await connection.ExecuteAsync(query, new
                        {
                            CategoryName = auction.Lot.Category.Name,
                            LotName = auction.Lot.Name,
                            auction.Lot.PaintingDate,
                            auction.Lot.Photo,
                            auction.Lot.Description,
                            auction.SellerId,
                            auction.StartBillingDateTime,
                            auction.EndBillingDateTime,
                            auction.StartPrice,
                            auction.CurrentPrice,
                            auction.FullPrice,
                            auction.BidStep
                        }, transaction);

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task AddBidAsync(Bid bid)
        {
            var query = @"
                INSERT INTO [dbo].[bid] (
                     [user_id]
                    ,[auction_id]
                    ,[date_time]
                    ,[sum]
                )
                VALUES (
                     @UserId
	                ,@AuctionId
	                ,GETDATE()
	                ,@Sum
                )";

            await using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            await connection.ExecuteAsync(query, new
            {
                bid.UserId,
                bid.AuctionId,
                bid.Sum
            });
        }

        public async Task AddMessageAsync(Message message)
        {
            var query = @"
                INSERT INTO [dbo].[message] (
	                 [user_id]
                    ,[auction_id]
                    ,[date_time]
                    ,[message_text]
                    ,[is_admin]
                )
                VALUES (
	                 @UserId
	                ,@AuctionId
	                ,GETDATE()
	                ,@MessageText
	                ,@IsAdmin
                )";

            await using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            await connection.ExecuteAsync(query, new
            {
                message.UserId,
                message.AuctionId,
                message.MessageText,
                message.IsAdmin
            });
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var query = @"
                SELECT
                     [category_id] AS CategoryId
                    ,[name]
                FROM [dbo].[category]
                ORDER BY [name]";

            await using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            return await connection.QueryAsync<Category>(query);
        }
    }
}