using AdTechAPI.CampaignsCache;

namespace AdTechAPI.Extensions
{
    static class AppServicesExtensions
    {
        public static IServiceCollection AddCacheBuildersService(this IServiceCollection services)
        {
            services.AddScoped<BuildActiveCampaignsCache, BuildActiveCampaignsCache>();

            return services;

        }
    }
}