using System.Reflection;
using ArtAuction.Infrastructure.Persistence;
using ArtAuction.Tests.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

namespace ArtAuction.Infrastructure.IntegrationTests.DataAttributes
{
    public class RemoveUserAfterAttribute : BeforeAfterTestAttribute
    {
        private readonly string _login;
        
        public RemoveUserAfterAttribute(string login)
        {
            _login = login;
        }
        
        public override void After(MethodInfo methodUnderTest)
        {
            var query = @"
                DECLARE @UserId UNIQUEIDENTIFIER

                SELECT @UserId = [user_id] 
                FROM [dbo].[user]
                WHERE 
	                [login] = @Login
                
                DELETE FROM [dbo].[account]
                WHERE [user_id] = @UserId

                DELETE FROM [dbo].[user] 
                WHERE [user_id] = @UserId";
            
            using (var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(query, new { Login = _login }, transaction);
                    transaction.Commit();
                }
            }
        }
    }
}