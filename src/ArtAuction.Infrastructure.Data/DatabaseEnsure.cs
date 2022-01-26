using Dapper;
using Microsoft.Data.SqlClient;

namespace ArtAuction.Infrastructure.Persistence
{
    public static class DatabaseEnsure
    {
        private const string DbName = "ArtAuction";
        
        public static void Run(string dbConnectionString)
        {
            using var connection = new SqlConnection(dbConnectionString);
            
            var exist = connection.ExecuteScalar<bool>("SELECT 1 FROM sys.databases WHERE name = @DbName", new { DbName });
            if (!exist)
            {
                connection.Execute("CREATE DATABASE @DbName", new { DbName });
            }
        }
    }
}