using AdTechAPI.Extensions;
using AdTechAPI.Models.DTOs;
using AdTechAPI.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Hangfire;
using Hangfire.PostgreSql; // Optional if you want to persist jobs in Postgres


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
builder.Services.AddInfraServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.RegisterBackgroundServices();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<CreatePlacementRequest>, CreatePlacementRequestValidator>();

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



app.UseHangfireDashboard();
// Seed the database
if (app.Environment.IsDevelopment())
{
    await app.SeedDatabaseAsync();
}


await app.RunAsync();

