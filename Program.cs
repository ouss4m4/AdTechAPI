using AdTechAPI.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for file logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logs to console
    .WriteTo.File("Logs/myapp.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7) // Logs to file
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

await app.RunAsync();
