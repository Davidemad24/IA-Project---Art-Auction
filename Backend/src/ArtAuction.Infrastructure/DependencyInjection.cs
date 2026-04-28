using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using ArtAuction.Application.Entities;
using ArtAuction.Infrastructure.Persistence;
using ArtAuction.Infrastructure.Repositories;
using ArtAuction.Infrastructure.Services;
using ArtAuction.Infrastructure.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
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
        
        // Identity service and SignalR
        services.AddIdentity<ApplicationUser, IdentityRole<int>>().AddEntityFrameworkStores<AppDbContext>();
       services.AddSingleton<IUserIdProvider, ClaimUserIdProvider>();
        services.AddSignalR();
        
        // Repositories DI
        services.AddScoped<IArtworkPostRepo, ArtworkPostRepo>();
        services.AddScoped<IPostBidRepo, PostBidRepo>();
        services.AddScoped<IPostSoldRepo, PostSoldRepo>();
        services.AddScoped<ITagRepo, TagRepo>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        services.AddScoped<IWatchListRepo, WatchListRepo>();
        services.AddScoped<IAdminRepo, AdminRepo>();
        services.AddScoped<IArtistRepo, ArtistRepo>();
        services.AddScoped<IBuyerRepo, BuyerRepo>();
        services.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();
        services.AddScoped<IAuthenticationRepo, AuthenticationRepo>();
        services.AddScoped<ISaveChanges, SaveChanges>();
        
        // External service registration
        services.AddScoped<IEmailServices, EmailServices>();
        services.AddScoped<INotificationServices, NotificationServices>();
            
        // Return services
        return services;
    }
}