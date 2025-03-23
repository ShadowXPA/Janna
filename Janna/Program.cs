using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;

namespace Janna;

class Program
{
    static async Task Main(string[] args)
    {
        string discordBotToken = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN")
            ?? throw new Exception("Environment variable 'DISCORD_BOT_TOKEN' is not set!");

        IServiceCollection serviceCollection = new ServiceCollection();

        // serviceCollection.AddSingleton<HttpClient>();
        // serviceCollection.AddSingleton<WeatherService>();

        DiscordClient discord = new(new DiscordConfiguration()
        {
            Token = discordBotToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });

        IServiceProvider services = serviceCollection.BuildServiceProvider();

        SlashCommandsExtension commands = discord.UseSlashCommands(new SlashCommandsConfiguration()
        {
            Services = services
        });

        // commands.RegisterCommands<WeatherCommandModule>();

        await discord.ConnectAsync();
        await Task.Delay(-1);
    }
}
