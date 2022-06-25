using System.Reflection;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArtAuction.Infrastructure.Persistence
{
    public static class DependencyInjectionPersistenceExtension
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // add dependencies

            services
                .AddFluentMigratorCore()
                .AddLogging(l => l.AddFluentMigratorConsole())
                .ConfigureRunner(r => r
                    .AddSqlServer()
                    .WithGlobalConnectionString(configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection))
                    .ScanIn(Assembly.GetExecutingAssembly()).For.All());

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();

            return services;
        }
    }
}