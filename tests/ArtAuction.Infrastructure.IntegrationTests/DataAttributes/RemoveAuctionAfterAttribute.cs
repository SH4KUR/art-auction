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
        private readonly int _auctionNumber;
        
        public RemoveAuctionAfterAttribute(int auctionNumber)
        {
            _auctionNumber = auctionNumber;
        }
        
        public override void After(MethodInfo methodUnderTest)
        {
            var query = @"
                DECLARE @AuctionId UNIQUEIDENTIFIER

                SELECT @AuctionId = [auction_id] 
                FROM [dbo].[auction]
                WHERE 
	                [auction_number] = @AuctionNumber
                
                DELETE FROM [dbo].[lot] 
                WHERE 
                    [lot_id] = (SELECT TOP 1 [lot_id] WHERE [auction_id] = @AuctionId)

                DELETE FROM [dbo].[auction]
                WHERE 
                    [auction_id] = @AuctionId";
            
            using (var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(query, new
                    {
                        AuctionNumber = _auctionNumber
                    }, transaction);
                    transaction.Commit();
                }
            }
        }
    }
}