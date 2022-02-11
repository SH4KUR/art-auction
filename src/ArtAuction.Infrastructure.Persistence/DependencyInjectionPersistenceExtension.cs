using System.Reflection;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace ArtAuction.Infrastructure.Persistence
{
    public static class DependencyInjectionPersistenceExtension
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, string dbConnectionString)
        {
            // add dependencies

            services
                .AddFluentMigratorCore()
                .AddLogging(l => l.AddFluentMigratorConsole())
                .ConfigureRunner(r => r
                    .AddSqlServer()
                    .WithGlobalConnectionString(dbConnectionString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.All());

            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }
    }
}