using DSharpPlus;
using DSharpPlus.SlashCommands;
using Janna.Geocoding;
using Janna.Utils;
using Janna.Weather;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Janna;

public class DiscordBot
{
    private readonly string _discordBotToken;

    public DiscordBot(string discordBotToken)
    {
        _discordBotToken = discordBotToken;
    }

    public Task ConnectAsync()
    {
        DiscordClient discord = new(new DiscordConfiguration()
        {
            Token = _discordBotToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });

        discord.Logger.LogInformation("Setting up service collection...");
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<ILogger>(discord.Logger);
        serviceCollection.AddSingleton<HttpClient>();
        serviceCollection.AddSingleton<IGeocodingService, GeocodingService>();
        serviceCollection.AddSingleton<IWeatherService, WeatherService>();

        IServiceProvider services = serviceCollection.BuildServiceProvider();
        discord.Logger.LogInformation("Service collection set up");

        var commands = discord.UseSlashCommands(new SlashCommandsConfiguration()
        {
            Services = services
        });

        discord.Logger.LogInformation("Registering commands...");
        commands.RegisterCommands<GeocodingCommandModule>(Constants.GetDiscordGuildId());
        commands.RegisterCommands<WeatherCommandModule>(Constants.GetDiscordGuildId());
        discord.Logger.LogInformation("Commands registered");

        discord.Logger.LogInformation("Connecting to Discord...");
        return discord.ConnectAsync();
    }
}
