using ArtAuction.Application;
using ArtAuction.Infrastructure;
using ArtAuction.Infrastructure.SignalR;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Controller Configuration
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add Infrastructure services to program
builder.Services.AddInfrastructure(builder.Configuration);

// Add application services to program
builder.Services.AddApplicationServices(builder.Configuration);

// Authorization
builder.Services.AddAuthorization();

// Hangfire configurations
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(
            builder.Configuration.GetConnectionString("SqlServer"),
            new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.FromSeconds(15),
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

builder.Services.AddHangfireServer();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Front end URL
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Required if you send cookies/auth headers
        });
});

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// SignalR Hub and Hangfire dashboard
app.MapHub<AuctionHub>("/auctionHub");
app.UseHangfireDashboard("/hangfire");

// Run app
app.Run();
