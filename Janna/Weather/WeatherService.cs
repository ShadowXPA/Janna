using System.Net.Http.Json;
using System.Text.Json;
using Janna.Extensions;
using Janna.Geocoding;
using Janna.OpenWeatherMap;
using Janna.Utils;

namespace Janna.Weather;

public class WeatherService : IWeatherService
{
    private HttpClient _httpClient;
    private IGeocodingService _geocodingService;

    public WeatherService(HttpClient httpClient, IGeocodingService geocodingService)
    {
        _httpClient = httpClient;
        _geocodingService = geocodingService;
    }

    public async Task<CurrentWeather?> GetCurrentWeatherAsync(string query, Units units)
    {
        var location = (await _geocodingService.GetLocationsAsync(query))?.FirstOrDefault();

        if (location is null) return null;

        var coordinates = location.Coordinates;
        var httpResponse = await _httpClient.GetAsync(Constants.Endpoint.GetCurrentWeatherEndpoint(coordinates.Latitude, coordinates.Longitude, units, Constants.GetEnvironmentVariableOrThrow(Constants.OPEN_WEATHER_MAP_TOKEN)));

        if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK) return null;

        var response = await httpResponse.Content.ReadFromJsonAsync<CurrentWeatherResponse>(JsonUtils.SNAKE_CASE_OPTIONS);

        if (response is null) return null;

        return response.ToCurrentWeather(location, units);
    }

    public async Task<WeatherForecast?> GetWeatherForecastAsync(string query, Units units)
    {
        var location = (await _geocodingService.GetLocationsAsync(query))?.FirstOrDefault();

        if (location is null) return null;

        var coordinates = location.Coordinates;
        var httpResponse = await _httpClient.GetAsync(Constants.Endpoint.GetWeatherForecastEndpoint(coordinates.Latitude, coordinates.Longitude, units, Constants.GetEnvironmentVariableOrThrow(Constants.OPEN_WEATHER_MAP_TOKEN)));

        if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK) return null;

        var response = await httpResponse.Content.ReadFromJsonAsync<WeatherForecastResponse>(JsonUtils.SNAKE_CASE_OPTIONS);

        if (response is null) return null;
 
        return response.ToWeatherForecast(location, units);
    }
}
