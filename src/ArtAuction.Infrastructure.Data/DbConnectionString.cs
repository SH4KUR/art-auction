using System.Configuration;

namespace ArtAuction.Infrastructure.Persistence
{
    public static class DbConnectionString
    {
        public static string ArtAuction => GetConnectionString("");

        private static string GetConnectionString(string connectionName)
        {
            if (ConfigurationManager.ConnectionStrings[connectionName] == null)
            {
                throw new ConfigurationErrorsException($"Connection string '{connectionName}' to database was not found");
            }

            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
    }
}
