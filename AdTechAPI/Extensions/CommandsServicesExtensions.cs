using AdTechAPI.CLI;

namespace AdTechAPI.Extensions
{
    static class CommandsServicesExtensions
    {

        public static IServiceCollection RegisterCommands(this IServiceCollection services)
        {
            services.AddSingleton<BuildActiveCampaignsCacheCommand>();
            return services;
        }
    }
}