using System.Collections.Immutable;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Janna.Extensions;
using Janna.Utils;
using Microsoft.Extensions.Logging;

namespace Janna.Weather;

public class WeatherCommandModule : ApplicationCommandModule
{
    private readonly ILogger _logger;
    private readonly IWeatherService _weatherService;

    public WeatherCommandModule(ILogger logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    [SlashCommand("weather", "Get the current weather for a specific location")]
    public async Task GetCurrentWeather(InteractionContext ctx,
        [Option("location", "The location you want to query")] string location,
        [Option("units", "The units you want the weather to show")] Units units = Units.Metric)
    {
        await ctx.DeferAsync();

        _logger.LogInformation("User '{user}' used the 'weather' command", ctx.User.Username);

        var weather = await _weatherService.GetCurrentWeatherAsync(location, units);
        var builder = new DiscordWebhookBuilder();

        if (weather is null)
        {
            await ctx.EditResponseAsync(builder.WithContent("Current Weather information was not found! Perhaps the location is invalid?"));
            return;
        }

        var embedBuilder = new DiscordEmbedBuilder();
        builder.AddEmbed(embedBuilder
            .WithColor(weather.Icon.Contains('n') ? DiscordColor.PhthaloBlue : DiscordColor.Azure)
            .WithAuthor($"{weather.Location.Name} (Lat: {weather.Location.Coordinates.Latitude}, Lon: {weather.Location.Coordinates.Longitude})",
                iconUrl: Constants.Endpoint.GetCountryFlagEndpoint(weather.Location.CountryCode))
            .WithTitle($"{weather.Condition} ({weather.Description})")
            .WithThumbnail(Constants.Endpoint.GetWeatherIconEndpoint(weather.Icon))
            .AddField("Temperature", $":thermometer: {((float)Math.Round(weather.Temperature)).ToTemperatureString(units)}", true)
            .AddField("Feels Like", $":thermometer: {((float)Math.Round(weather.FeelsLike)).ToTemperatureString(units)}", true)
            .AddField("Humidity", $":droplet: {weather.Humidity}%", true)
            .AddField("Wind", $":dash: {weather.WindSpeed.ToWindSpeedString(units)} ({weather.WindDirection.ToWindDirectionString()})")
            .AddField("Pressure (Sea Level)", $":ocean: {weather.PressureSeaLevel} hPa", true)
            .AddField("Pressure (Ground Level)", $":rock: {weather.PressureGroundLevel} hPa", true)
            .AddField("Cloudiness", $":cloud: {weather.Cloudiness}%")
            .AddField("Visibility", $":eye: {weather.Visibility.ToVisibilityString()}")
            .AddField("Sunrise", $":sunrise: {weather.Sunrise.ToLongTime()}")
            .AddField("Sunset", $":sunset: {weather.Sunset.ToLongTime()}")
            .WithFooter($"Local time: {weather.DateTime.DayOfWeek}, {weather.DateTime.ToIso8601String()}")
            .Build());

        await ctx.EditResponseAsync(builder);
    }

    [SlashCommand("forecast", "Get the forecast for the specified day in 3-hour steps")]
    public async Task GetWeatherForecast(InteractionContext ctx,
        [Option("location", "The location you want to query")] string location,
        [Option("day", "The day of the week you want to view the forecast")] DayOfWeek dayOfWeek,
        [Option("units", "The units you want the weather to show")] Units units = Units.Metric)
    {
        await ctx.DeferAsync();

        _logger.LogInformation("User '{user}' used the 'forecast' command", ctx.User.Username);

        var forecast = await _weatherService.GetWeatherForecastAsync(location, units);
        var builder = new DiscordWebhookBuilder();

        if (forecast is null || forecast.Forecast.Count == 0)
        {
            await ctx.EditResponseAsync(builder.WithContent("Weather Forecast information was not found! Perhaps the location is invalid?"));
            return;
        }

        var forecastByDayOfWeek = forecast.Forecast.GroupBy(f => f.DateTime.DayOfWeek);
        var dayForecast = forecastByDayOfWeek.FirstOrDefault(f => f.Key == dayOfWeek);

        if (dayForecast is null)
        {
            await ctx.EditResponseAsync(builder.WithContent($"There is no Weather Forecast for {dayOfWeek}."));
            return;
        }

        var dateTime = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromSeconds(forecast.Timezone));
        var embedBuilder = new DiscordEmbedBuilder();
        embedBuilder.WithTitle($"{dayOfWeek}")
            .WithAuthor($"{forecast.Location.Name} (Lat: {forecast.Location.Coordinates.Latitude}, Lon: {forecast.Location.Coordinates.Longitude})",
                iconUrl: Constants.Endpoint.GetCountryFlagEndpoint(forecast.Location.CountryCode))
            .WithColor(dayOfWeek.ToColor());

        foreach (var f in dayForecast)
        {
            var sb = new StringBuilder();

            sb.Append($"{f.Condition} ({f.Description})\n");
            sb.Append($":thermometer: **{((float)Math.Round(f.Temperature)).ToTemperatureString(units)}** (Feels like: {((float)Math.Round(f.FeelsLike)).ToTemperatureString(units)})\n");
            sb.Append($":droplet: **Humidity:** {f.Humidity}%\n");
            sb.Append($":droplet: **Precipitation:** {f.Percipitation}%\n");
            sb.Append($":dash: **Wind:** {f.WindDirection.ToWindDirectionEmoji()} {f.WindSpeed.ToWindSpeedString(units)}\n");
            sb.Append($":ocean: **Pressure (Sea Level):** {f.PressureSeaLevel} hPa\n");
            sb.Append($":rock: **Pressure (Ground Level):** {f.PressureGroundLevel} hPa\n");
            sb.Append($":cloud: **Cloudiness:** {f.Cloudiness}%\n");
            sb.Append($":eye: **Visibility:** {f.Visibility.ToVisibilityString()}");

            embedBuilder.AddField(f.DateTime.ToShortTime(), sb.ToString(), true);
        }

        embedBuilder.WithFooter($"Local time: {dateTime.DayOfWeek}, {dateTime.ToIso8601String()}");
        await ctx.EditResponseAsync(builder.AddEmbed(embedBuilder.Build()));
    }
}
