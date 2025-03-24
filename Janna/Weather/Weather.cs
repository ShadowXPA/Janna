using Janna.Geocoding;

namespace Janna.Weather;

public class CurrentWeather
{
    public required Location Location { get; set; }
    public Units Units { get; set; }
    public required string Condition { get; set; }
    public required string Description { get; set; }
    public required string Icon { get; set; }
    public float Temperature { get; set; }
    public float FeelsLike { get; set; }
    public int PressureSeaLevel { get; set; }
    public int PressureGroundLevel { get; set; }
    public int Humidity { get; set; }
    public int Cloudiness { get; set; }
    public float WindSpeed { get; set; }
    public int WindDirection { get; set; }
    public int Visibility { get; set; }
    public DateTimeOffset Sunrise { get; set; }
    public DateTimeOffset Sunset { get; set; }
    public DateTimeOffset DateTime { get; set; }
}
