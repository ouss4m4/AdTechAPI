using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AdTechAPI.BackgroundServices;

namespace AdTechAPI.Extensions
{
    public static class BackgroundServiceExtensions
    {
        public static IServiceCollection RegisterBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<CampaignsPool>();
            services.AddHostedService<GenerateRollupsSchedule>();


            // Resolve ILoggerFactory to create a logger
            using var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("BackgroundServiceExtensions");

            logger.LogInformation("Background Service Registered");


            return services;
        }
    }
}