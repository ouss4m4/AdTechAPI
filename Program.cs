using AdTechAPI.CacheBuildersServices;
using AdTechAPI.CLI;
using AdTechAPI.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for file logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logs to console
    .WriteTo.File("Logs/main-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
.CreateLogger();

// Add Serilog to the logging system
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AdTech API",
        Version = "v1",
        Description = "AdTech API for managing campaigns, platforms, and verticals"
    });
});

// ADD SERVICES (db,cache,auth...)
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddJwtService(builder.Configuration);
builder.Services.RegisterCommands();
builder.Services.AddCacheBuildersService();
// ADD BACKGROUND SERVICES
builder.Services.RegisterBackgroundServices();



// HTTP 
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdTech API V1");
        c.RoutePrefix = "swagger";
    });
}

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed the database
if (app.Environment.IsDevelopment())
{
    await app.SeedDatabaseAsync();
}


// Check if it's a CLI command
// if (args.Length > 0)
// TODO: Move commands to separate solution
if (args.Length > 0 && args[0] == "cache:build-active-campaigns-cache")
{
    Console.WriteLine("---------------args");
    Console.WriteLine(args[0]);

    using var scope = app.Services.CreateScope();
    var cacheBuilder = scope.ServiceProvider.GetRequiredService<BuildActiveCampaignsCache>();

    Console.WriteLine("Running cache update from CLI...");
    await cacheBuilder.Run();
    Console.WriteLine("Campaign cache updated via CLI.");

    return; // Exit after running the command
}

await app.RunAsync();

