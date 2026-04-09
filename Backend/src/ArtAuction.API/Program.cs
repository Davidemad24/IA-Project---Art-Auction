using ArtAuction.Application;
using ArtAuction.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Controller Configuration
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add Infrastructure services to program
builder.Services.AddInfrastructure(builder.Configuration);

// Add application services to program
builder.Services.AddApplicationServices();

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
