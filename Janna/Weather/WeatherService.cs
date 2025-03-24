using System.Net.Http.Json;
using System.Text.Json;
using Janna.Extensions;
using Janna.Geocoding;
using Janna.OpenWeatherMap;
using Janna.Utils;
using Microsoft.Extensions.Logging;

namespace Janna.Weather;

public class WeatherService : IWeatherService
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;
    private readonly IGeocodingService _geocodingService;

    public WeatherService(ILogger logger, HttpClient httpClient, IGeocodingService geocodingService)
    {
        _logger = logger;
        _httpClient = httpClient;
        _geocodingService = geocodingService;
    }

    public async Task<CurrentWeather?> GetCurrentWeatherAsync(string query, Units units)
    {
        _logger.LogInformation("Start fetching current weather for '{query}' in {units}...", query, units);

        var location = (await _geocodingService.GetLocationsAsync(query))?.FirstOrDefault();

        if (location is null)
        {
            _logger.LogInformation("No location was found...");
            return null;
        }

        _logger.LogInformation("Found location: {location}", location.Name);

        var coordinates = location.Coordinates;

        _logger.LogInformation("Fetching current weather for '{query}' in {units}...", query, units);
        var httpResponse = await _httpClient.GetAsync(Constants.Endpoint.GetCurrentWeatherEndpoint(coordinates.Latitude, coordinates.Longitude, units, Constants.GetEnvironmentVariableOrThrow(Constants.OPEN_WEATHER_MAP_TOKEN)));

        if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK) return null;

        _logger.LogInformation("Fetched current weather");
        _logger.LogInformation("Parsing current weather response...");
        var response = await httpResponse.Content.ReadFromJsonAsync<CurrentWeatherResponse>(JsonUtils.SNAKE_CASE_OPTIONS);

        if (response is null) return null;

        _logger.LogInformation("Current weather response parsed");
        return response.ToCurrentWeather(location, units);
    }

    public async Task<WeatherForecast?> GetWeatherForecastAsync(string query, Units units)
    {
        _logger.LogInformation("Start fetching weather forecast for '{query}' in {units}", query, units);
        var location = (await _geocodingService.GetLocationsAsync(query))?.FirstOrDefault();

        if (location is null)
        {
            _logger.LogInformation("No location was found...");
            return null;
        }

        _logger.LogInformation("Fetching weather forecast for '{query}' in {units}", query, units);
        var coordinates = location.Coordinates;
        var httpResponse = await _httpClient.GetAsync(Constants.Endpoint.GetWeatherForecastEndpoint(coordinates.Latitude, coordinates.Longitude, units, Constants.GetEnvironmentVariableOrThrow(Constants.OPEN_WEATHER_MAP_TOKEN)));

        if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK) return null;

        _logger.LogInformation("Fetched weather forecast");
        _logger.LogInformation("Parsing weather forecast response...");
        var response = await httpResponse.Content.ReadFromJsonAsync<WeatherForecastResponse>(JsonUtils.SNAKE_CASE_OPTIONS);

        if (response is null) return null;

        _logger.LogInformation("Weather forecast response parsed");
        return response.ToWeatherForecast(location, units);
    }
}
