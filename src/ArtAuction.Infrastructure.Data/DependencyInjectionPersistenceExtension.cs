using ArtAuction.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArtAuction.Infrastructure.Persistence
{
    public static class DependencyInjectionPersistenceExtension
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, string connectionString)
        {
            // add dependencies
            
            services
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString))
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }
    }
}