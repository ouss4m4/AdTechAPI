using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AdTechAPI.Services;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Extensions
{
    public static class InfraServiceExtensions
    {

        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                           .AddJwtBearer(options =>
                           {
                               options.TokenValidationParameters = new TokenValidationParameters
                               {
                                   ValidateIssuer = true,
                                   ValidateAudience = true,
                                   ValidateLifetime = true,
                                   ValidateIssuerSigningKey = true,
                                   ValidIssuer = configuration["Jwt:Issuer"],
                                   ValidAudience = configuration["Jwt:Audience"],
                                   IssuerSigningKey = new SymmetricSecurityKey(
                                       Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ??
                                           throw new InvalidOperationException("JWT Key not found")))
                               };

                               // Map our custom claims to standard identity claims
                               options.MapInboundClaims = false;  // Disable default claim mapping
                               options.TokenValidationParameters.NameClaimType = "name";
                               options.TokenValidationParameters.RoleClaimType = "role";
                           });

            services.AddScoped<IJwtService, JwtService>();

            var connectionString = configuration.GetConnectionString("PostgresDb");
            // Enable dynamic JSON serialization
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build(); // Build the configured data source

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(dataSource));

            services.AddScoped<IJwtService, JwtService>();

            services.AddSingleton<RedisService>();
            return services;
        }

    }
}