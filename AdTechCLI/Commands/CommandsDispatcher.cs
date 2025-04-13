using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdTechCLI.Commands
{
    public class CommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _commands = new();

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _commands.Add("cache:build-active-campaigns", typeof(BuildActiveCampaignsCacheCommand));
            _commands.Add("cache:build-active-placements", typeof(BuildPlacementsCacheCommands));
        }

        public async Task ExecuteAsync(string[] args)
        {
            if (args.Length == 0 || !_commands.ContainsKey(args[0]))
            {
                Console.WriteLine("Invalid or missing command.");
                return;
            }

            var commandType = _commands[args[0]];
            var commandInstance = (ICommandHandler)_serviceProvider.GetRequiredService(commandType);
            await commandInstance.RunAsync();
        }
    }
}