using System.Reflection;
using ArtAuction.Core.Application.Interfaces.Services;
using ArtAuction.Core.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ArtAuction.Core.Application
{
    public static class DependencyInjectionApplicationExtension
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            // add dependencies

            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ApplicationLayerMappingProfile)));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IPasswordService, PasswordService>();

            return services;
        }
    }
}