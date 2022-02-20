using System.Reflection;
using ArtAuction.Tests.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

namespace ArtAuction.Infrastructure.IntegrationTests.DataAttributes
{
    public class UseCategoryAttribute : BeforeAfterTestAttribute
    {
        public string CategoryId { get; set; } = "{C33144F0-CED6-4B81-B7FF-58ADAA38D284}";
        public string CategoryName { get; set; } = "Some Test CategoryName";

        public override void Before(MethodInfo methodUnderTest)
        {
            After(methodUnderTest);

            var query = @"
                INSERT INTO [dbo].[category] (
                     [category_id]
                    ,[name]
                )
                VALUES (
                     @CategoryId
	                ,@CategoryName
                )";

            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString("ArtAuctionDbConnection"));
            connection.Execute(query, new
            {
                CategoryId,
                CategoryName
            });
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var query = @"
                DELETE FROM [dbo].[category]
                WHERE
                    [category_id] = @CategoryId";

            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString("ArtAuctionDbConnection"));
            connection.Execute(query, new
            {
                CategoryId
            });
        }
    }
}