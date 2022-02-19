using System;
using System.Reflection;
using ArtAuction.Tests.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

namespace ArtAuction.Infrastructure.IntegrationTests.DataAttributes
{
    public class UseBidAttribute : BeforeAfterTestAttribute
    {
        public string UserId { get; set; } = "{C33144F0-CED6-4B81-B7FF-58ADAA38D284}";
        public string AuctionId { get; set; } = "{B8315CA0-2A29-49BA-9F2F-318CED12746E}";
        public string DateTime { get; set; } = "2022-01-01 12:30:30";
        
        public override void Before(MethodInfo methodUnderTest)
        {
            After(methodUnderTest);

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
	                ,@DateTime
	                ,1450
                )";

            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString("ArtAuctionDbConnection"));
            connection.Execute(query, new
            {
                UserId,
                AuctionId,
                DateTime
            });
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var query = @"
                DELETE FROM [dbo].[bid]
                WHERE
                      [auction_id] = @AuctionId
                  AND [date_time] = @DateTime";

            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString("ArtAuctionDbConnection"));
            connection.Execute(query, new
            {
                AuctionId,
                DateTime
            });
        }
    }
}