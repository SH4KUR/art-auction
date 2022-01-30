using ArtAuction.Core.Base.Interfaces;
using ArtAuction.Infrastructure.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArtAuction.Infrastructure.Services
{
    public static class DependencyInjectionServiceExtension
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            // add dependencies

            services.AddScoped<IPasswordService, PasswordService>();
            
            return services;
        }
    }
}