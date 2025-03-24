
using System.Net.Http.Json;
using Janna.Extensions;
using Janna.OpenWeatherMap;
using Janna.Utils;
using Microsoft.Extensions.Logging;

namespace Janna.Geocoding;

public class GeocodingService : IGeocodingService
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    public GeocodingService(ILogger logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<List<Location>?> GetLocationsAsync(string query, int l = 1)
    {
        int limit = Math.Max(0, Math.Min(l, 5));

        _logger.LogInformation("Fetching location for '{query}' limiting to {limit}", query, limit);
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(Constants.Endpoint.GetGeocodingEndpoint(query, limit, Constants.GetEnvironmentVariableOrThrow(Constants.OPEN_WEATHER_MAP_TOKEN)));

        if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK) return null;

        _logger.LogInformation("Parsing location response...");
        var response = await httpResponse.Content.ReadFromJsonAsync<List<GeocodingResponse>>(JsonUtils.SNAKE_CASE_OPTIONS);

        if (response is null) return null;

        _logger.LogInformation("Parsed location response");
        var locations = new List<Location>(response.Count);
        response.ForEach(geoResponse => locations.Add(geoResponse.ToLocation()));
        _logger.LogInformation("Found {count} location(s)", locations.Count);

        return locations;
    }
}
