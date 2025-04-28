using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AdTechAPI.CampaignsCache;
using AdTechAPI.Services;
using AdTechAPI.Rollups;
using AdTechAPI.PlacementCache;
using AdTechAPI.ClickServices;
using AdTechAPI.CachedData;

namespace AdTechAPI.Extensions
{
    static class AppServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            var geoPath = Path.Combine(Directory.GetCurrentDirectory(), "GeoIP", "GeoLite2-Country.mmdb");
            services.AddSingleton(new GeoIPService(geoPath));

            services.AddScoped<BuildActiveCampaignsCache>();
            services.AddScoped<GenerateRollupHour>();
            services.AddScoped<BuildPlacementCache>();
            services.AddScoped<ClickService>();
            services.AddScoped<ClickPlacementService>();
            services.AddScoped<ClickCampaignService>();
            services.AddScoped<CountriesCache>();


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
