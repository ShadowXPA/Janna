
using System.Net.Http.Json;
using Janna.Extensions;
using Janna.OpenWeatherMap;
using Janna.Utils;

namespace Janna.Geocoding;

public class GeocodingService : IGeocodingService
{
    private HttpClient _httpClient;

    public GeocodingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Location>?> GetLocationsAsync(string query, int l = 1)
    {
        int limit = Math.Max(0, Math.Min(l, 5));

        HttpResponseMessage httpResponse = await _httpClient.GetAsync(Constants.Endpoint.GetGeocodingEndpoint(query, limit, Constants.GetEnvironmentVariableOrThrow(Constants.OPEN_WEATHER_MAP_TOKEN)));

        if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK) return null;

        var response = await httpResponse.Content.ReadFromJsonAsync<List<GeocodingResponse>>(JsonUtils.SNAKE_CASE_OPTIONS);

        if (response is null) return null;

        var locations = new List<Location>(response.Count);
        response.ForEach(geoResponse => locations.Add(geoResponse.ToLocation()));

        return locations;
    }
}
