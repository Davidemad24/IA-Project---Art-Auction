using System.Reflection;
using System.Text;
using ArtAuction.API.Configurations;
using ArtAuction.Application.Interfaces.Services;
using ArtAuction.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ArtAuction.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Register services
        services.AddScoped<IArtworkPostServices, ArtworkPostServices>();
        services.AddScoped<IPostSoldServices, PostSoldServices>();
        services.AddScoped<IPostBidServices, PostBidServices>();
        services.AddScoped<ITagServices, TagServices>();
        services.AddScoped<ICategoryServices, CategoryServices>();
        services.AddScoped<IWatchListServices, WatchListServices>();
        services.AddScoped<IAuthenticationServices, AuthenticationServices>();
        services.AddScoped<IJwtServices, JwtServices>();
        services.AddScoped<IProfileServices, ProfileServices>();
        services.AddScoped<IArtistServices, ArtistServices>();
        services.AddScoped<IAuctionServices, AuctionServices>();
        
        // Jwt configurations
        var jwtConfig = new JwtConfiguration();
        configuration.GetSection("JWT").Bind(jwtConfig);
        services.AddSingleton(jwtConfig);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                };

                // --- Merged SignalR Event Logic ---
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        // Make sure this matches your Actual Hub Route
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/auctionHub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        
        // Register automapping 
        services.AddAutoMapper(config => {}, Assembly.GetExecutingAssembly());
        
        // Return services
        return services;
    }
}