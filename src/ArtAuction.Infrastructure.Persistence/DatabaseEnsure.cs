using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.Infrastructure.Persistence
{
    public static class DatabaseEnsure
    {
        private const string DbName = "ArtAuction";
        
        public static void Run(IConfiguration configuration)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            
            var exist = connection.ExecuteScalar<bool>("SELECT 1 FROM sys.databases WHERE name = @DbName", new { DbName });
            if (!exist)
            {
                connection.Execute("CREATE DATABASE @DbName", new { DbName });
            }
        }
    }
}