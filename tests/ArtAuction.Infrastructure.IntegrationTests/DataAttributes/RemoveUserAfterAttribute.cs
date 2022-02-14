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
                DELETE FROM [dbo].[user] 
                WHERE 
                    [login] = @Login";
            
            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            connection.Execute(query, new { Login = _login });
        }
    }
}