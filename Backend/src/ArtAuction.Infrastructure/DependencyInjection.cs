using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Identities;
using ArtAuction.Infrastructure.Persistence;
using ArtAuction.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArtAuction.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext service
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
            configuration.GetConnectionString("SqlServer"))
        );
        
        // Identity service
        services.AddIdentity<ApplicationUser, IdentityRole<int>>().AddEntityFrameworkStores<AppDbContext>();
        
        // Repositories DI
        services.AddScoped<IArtworkPostRepo, ArtworkPostRepo>();
        services.AddScoped<IPostBidRepo, PostBidRepo>();
        services.AddScoped<IPostBidRepo, PostBidRepo>();
        services.AddScoped<ISaveChanges, SaveChanges>();
            
        // Return services
        return services;
    }
}