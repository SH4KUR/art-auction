using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ArtAuction.WebUI
{
    public static class MigrationExtension
    {
        public static IApplicationBuilder Migrate(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            
            var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
            runner?.MigrateDown(202201251800); runner?.MigrateUp();

            // uncomment it for migration revert
            // runner?.MigrateDown(202201251200);

            return builder;
        }
    }
}
