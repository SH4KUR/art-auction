using ArtAuction.Core.Application.Interfaces.Services;
using ArtAuction.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArtAuction.Core.Application
{
    public static class DependencyInjectionApplicationExtension
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            // add dependencies
            
            services.AddScoped<IPasswordService, PasswordService>();

            return services;
        }
    }
}