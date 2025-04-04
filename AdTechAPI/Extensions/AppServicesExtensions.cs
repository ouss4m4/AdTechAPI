using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AdTechAPI.CampaignsCache;
using AdTechAPI.Services;
using AdTechAPI.Rollups;

namespace AdTechAPI.Extensions
{
    static class AppServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<BuildActiveCampaignsCache>();
            services.AddScoped<GenerateRollupHour>();

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

            return services;

        }
    }
}