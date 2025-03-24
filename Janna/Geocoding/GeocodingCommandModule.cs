using System.Text;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace Janna.Geocoding;

public class GeocodingCommandModule : ApplicationCommandModule
{
    private readonly ILogger _logger;
    private readonly IGeocodingService _geocodingService;

    public GeocodingCommandModule(ILogger logger, IGeocodingService geocodingService)
    {
        _logger = logger;
        _geocodingService = geocodingService;
    }

    [SlashCommand("location", "Get the latitude and longitute of the location")]
    public async Task GetLocations(InteractionContext ctx,
        [Option("query", "The location to search")] string query,
        [Option("limit", "Number of locations (Min: 0, Max: 5)")] long limit = 0)
    {
        await ctx.DeferAsync();

        _logger.LogInformation("User '{user}' used the 'location' command", ctx.User.Username);

        var locations = await _geocodingService.GetLocationsAsync(query, (int)limit);
        var builder = new DiscordWebhookBuilder();

        if (locations is null || locations.Count == 0)
        {
            await ctx.EditResponseAsync(builder.WithContent("No location was found!"));
            return;
        }

        var embedBuilder = new DiscordEmbedBuilder();
        var sb = new StringBuilder();
        embedBuilder.WithTitle($"Found locations for '{query}'")
            .WithColor(DiscordColor.Wheat)
            .WithFooter("Copy any of these when querying the weather.");

        foreach (var location in locations)
        {
            sb.Append(location.Name).Append('\n');
        }

        embedBuilder.WithDescription(sb.ToString());
        await ctx.EditResponseAsync(builder.AddEmbed(embedBuilder));
    }
}
