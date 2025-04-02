using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using AdTechAPI.CampaignsCache;
using AdTechAPI.Extensions;

namespace AdTechCLI.Commands
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder().Build();
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var dispatcher = serviceProvider.GetRequiredService<CommandDispatcher>();
            await dispatcher.ExecuteAsync(args);
        }

        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Load the configuration from the AdTechAPI project's appsettings.json file
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())  // Make sure we use the current directory of the CLI project
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // Pointing to the appsettings.json
                        .Build();

                    // Register API services (referencing AdTechAPI)
                    services.AddScoped<BuildActiveCampaignsCache>();
                    services.AddScoped<BuildActiveCampaignsCacheCommand>();

                    // Register command dispatcher
                    services.AddSingleton<CommandDispatcher>();

                    // Add the configuration to services so other components can access it
                    services.AddSingleton<IConfiguration>(configuration);

                    // Register any other necessary services that rely on configuration here
                    services.AddInfraServices(configuration); // This is where AppDbContext gets added

                });
    }
}