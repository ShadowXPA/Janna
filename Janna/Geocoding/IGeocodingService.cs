namespace Janna.Geocoding;

public interface IGeocodingService
{
    Task<List<Location>?> GetLocationsAsync(string query, int limit = 1);
}
