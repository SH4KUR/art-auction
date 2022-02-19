using System.Reflection;
using ArtAuction.Infrastructure.Persistence;
using ArtAuction.Tests.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

namespace ArtAuction.Infrastructure.IntegrationTests.DataAttributes
{
    public class RemoveAuctionAfterAttribute : BeforeAfterTestAttribute
    {
        private readonly string _sellerId;
        
        public RemoveAuctionAfterAttribute(string sellerId)
        {
            _sellerId = sellerId;
        }
        
        public override void After(MethodInfo methodUnderTest)
        {
            var query = @"
                DECLARE @AuctionInfo TABLE (
                     [auction_id] UNIQUEIDENTIFIER 
                    ,[lot_id] UNIQUEIDENTIFIER
                )

                INSERT INTO @AuctionInfo
                SELECT 
                     [auction_id] 
                    ,[lot_id] 
                FROM [dbo].[auction]
                WHERE 
	                [seller_id] = @SellerId
                
                DELETE FROM [dbo].[auction]
                WHERE 
                    [auction_id] = (SELECT TOP 1 [auction_id] FROM @AuctionInfo)
            
                DELETE FROM [dbo].[lot] 
                WHERE 
                    [lot_id] = (SELECT TOP 1 [lot_id] FROM @AuctionInfo)";
            
            using (var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(query, new
                    {
                        SellerId = _sellerId
                    }, transaction);
                    transaction.Commit();
                }
            }
        }
    }
}