using System.Reflection;
using ArtAuction.Tests.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

namespace ArtAuction.Infrastructure.IntegrationTests.DataAttributes
{
    public class UseMessageAttribute : BeforeAfterTestAttribute
    {
        public string UserId { get; set; } = "{C33144F0-CED6-4B81-B7FF-58ADAA38D284}";
        public string AuctionId { get; set; } = "{B8315CA0-2A29-49BA-9F2F-318CED12746E}";

        public override void Before(MethodInfo methodUnderTest)
        {
            After(methodUnderTest);

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
	                ,'Some test message text'
	                ,0
                )";

            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString("ArtAuctionDbConnection"));
            connection.Execute(query, new
            {
                UserId,
                AuctionId
            });
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var query = @"
                DELETE FROM [dbo].[message]
                WHERE 
                    [auction_id] = @AuctionId";

            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString("ArtAuctionDbConnection"));
            connection.Execute(query, new
            {
                AuctionId
            });
        }
    }
}