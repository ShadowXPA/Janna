using Janna.Geocoding;

namespace Janna.Weather;

public class WeatherForecast
{
    public required Location Location { get; set; }
    public Units Units { get; set; }
    public required List<Forecast> Forecast { get; set; }
    public long Timezone { get; set; }
}
