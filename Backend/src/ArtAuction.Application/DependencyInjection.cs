using System.Reflection;
using ArtAuction.Application.Interfaces.Services;
using ArtAuction.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArtAuction.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<IArtworkPostServices, ArtworkPostServices>();
        
        // Register automapping 
        services.AddAutoMapper(config => {}, Assembly.GetExecutingAssembly());
        
        // Return services
        return services;
    }
}