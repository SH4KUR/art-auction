using System;
using System.Reflection;
using ArtAuction.Infrastructure.Persistence;
using ArtAuction.Tests.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

namespace ArtAuction.Infrastructure.IntegrationTests.DataAttributes
{
    public class RemoveBidsAfterAttribute : BeforeAfterTestAttribute
    {
        private readonly Guid _auctionId;
        
        public RemoveBidsAfterAttribute(Guid auctionId)
        {
            _auctionId = auctionId;
        }
        
        public override void After(MethodInfo methodUnderTest)
        {
            var query = @"
                DELETE FROM [dbo].[bid] 
                WHERE 
                    [auction_id] = @AuctionId";

            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            connection.Execute(query, new
            {
                AuctionId = _auctionId
            });
        }
    }
}