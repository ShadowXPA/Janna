using DSharpPlus;
using DSharpPlus.SlashCommands;
using Janna.Geocoding;
using Janna.Utils;
using Janna.Weather;
using Microsoft.Extensions.DependencyInjection;

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
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<HttpClient>();
        serviceCollection.AddSingleton<IGeocodingService, GeocodingService>();
        serviceCollection.AddSingleton<IWeatherService, WeatherService>();

        DiscordClient discord = new(new DiscordConfiguration()
        {
            Token = _discordBotToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });

        IServiceProvider services = serviceCollection.BuildServiceProvider();

        var commands = discord.UseSlashCommands(new SlashCommandsConfiguration()
        {
            Services = services
        });

        commands.RegisterCommands<GeocodingCommandModule>(Constants.GetDiscordGuildId());
        commands.RegisterCommands<WeatherCommandModule>(Constants.GetDiscordGuildId());

        return discord.ConnectAsync();
    }
}
