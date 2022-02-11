using System.Reflection;
using ArtAuction.Tests.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

namespace ArtAuction.Infrastructure.IntegrationTests.DataAttributes
{
    public class UseRemoveUserAfterAttribute : BeforeAfterTestAttribute
    {
        private readonly string _login;
        
        public UseRemoveUserAfterAttribute(string login)
        {
            _login = login;
        }
        
        public override void After(MethodInfo methodUnderTest)
        {
            var query = @"
                DELETE FROM [dbo].[user] 
                WHERE 
                    [login] = @Login";
            
            using var connection = new SqlConnection(FakeConfig.Get().GetConnectionString("ArtAuctionDbConnection"));
            connection.Execute(query, new { Login = _login });
        }
    }
}