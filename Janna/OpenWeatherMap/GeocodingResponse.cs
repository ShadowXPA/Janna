using Janna.Geocoding;

namespace Janna.OpenWeatherMap;

public class GeocodingResponse
{
    public required string Name { get; set; }
    public string? State { get; set; }
    public required string Country { get; set; }
    public float Lat { get; set; }
    public float Lon { get; set; }
}
