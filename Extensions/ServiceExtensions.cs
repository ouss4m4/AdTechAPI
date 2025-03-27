using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AdTechAPI.Services;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
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

            return services;
        }

        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            // Add other services here
            return services;
        }

        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgresDb");
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }

        public static IServiceCollection AddAuthorizationPolicies(
            this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireClaim("role", "Admin"));

                options.AddPolicy("RequireStaffRole", policy =>
                    policy.RequireClaim("role", new[] { "Admin", "Moderator" }));

                options.AddPolicy("SameClientAccess", policy =>
                    policy.RequireClaim("cid")); // Will check if client IDs match in handler
            });

            return services;
        }
    }
}