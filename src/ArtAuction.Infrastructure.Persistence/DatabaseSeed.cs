using System.IO;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.Infrastructure.Persistence
{
    public static class DatabaseSeed
    {
        public static void Seed(IConfiguration configuration)
        {
            UsersSeed(configuration);
            CategoriesSeed(configuration);
        }

        private static void UsersSeed(IConfiguration configuration)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));

            var isAnyUserExist = connection.ExecuteScalar<bool>("SELECT 1 FROM [dbo].[user]");
            if (!isAnyUserExist)
            {
                var query = File.ReadAllText(@"..\ArtAuction.Infrastructure.Persistence\SqlScripts\SeedBasicUsers.sql");
                connection.Execute(query);
            }
        }

        private static void CategoriesSeed(IConfiguration configuration)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));

            var isAnyCategoryExist = connection.ExecuteScalar<bool>("SELECT 1 FROM [dbo].[category]");
            if (!isAnyCategoryExist)
            {
                var query = File.ReadAllText(@"..\ArtAuction.Infrastructure.Persistence\SqlScripts\SeedBasicCategories.sql");
                connection.Execute(query);
            }
        }
    }
}