using System.Reflection;
using ArtAuction.Tests.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

namespace ArtAuction.Infrastructure.IntegrationTests.DataAttributes
{
    public class UseUserAttribute : BeforeAfterTestAttribute
    {
        public string UserId { get; set; } = "{CF8F3639-7328-429E-9791-77972854F730}";
        public string Login { get; set; } = "user_test_login";
        public string Email { get; set; } = "user-repository.test@email.com";
        public string BirthDate { get; set; } = "1999-01-01";

        public override void Before(MethodInfo methodUnderTest)
        {
            After(methodUnderTest);

            var query = @"
                INSERT INTO [dbo].[user] (
                     [user_id]
	                ,[login]
	                ,[email]
	                ,[password]
	                ,[role]
	                ,[first_name]
	                ,[last_name]
	                ,[patronymic]
	                ,[birth_date]
	                ,[address]
	                ,[is_vip]
	                ,[is_blocked]
                )
                VALUES (
	                 @UserId
	                ,@Login
	                ,@Email
	                ,'B97BDCB0B4D1802A2E727FBE143CC631935BCA83C60019E1733620C08916095B'     -- Password1
	                ,1
	                ,'First Name'
	                ,'Second Name'
	                ,'Patronymic'
	                ,@BirthDate
	                ,'Some test address'
	                ,0
	                ,0
                )";

            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString("ArtAuctionDbConnection"));
            connection.Execute(query, new
            {
                UserId, 
                Login, 
                Email, 
                BirthDate
            });
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var query = @"
                DELETE FROM [dbo].[user] 
                WHERE 
                    [user_id] = @UserId";
            
            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString("ArtAuctionDbConnection"));
            connection.Execute(query, new
            {
                UserId
            });
        }
    }
}