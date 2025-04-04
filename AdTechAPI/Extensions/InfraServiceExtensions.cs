
using AdTechAPI.Services;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.PostgreSql;

namespace AdTechAPI.Extensions
{
    public static class InfraServiceExtensions
    {

        public static IServiceCollection AddInfraServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {


            var connectionString = configuration.GetConnectionString("PostgresDb");
            // Enable dynamic JSON serialization
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build(); // Build the configured data source

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(dataSource));
            services.AddHangfire(config =>
            config.UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(connectionString)));

            services.AddHangfireServer();

            services.AddScoped<IJwtService, JwtService>();

            services.AddSingleton<RedisService>();


            return services;
        }

    }
}