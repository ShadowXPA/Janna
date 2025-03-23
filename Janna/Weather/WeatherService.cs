
namespace Janna;

public class WeatherService : IWeatherService
{
    public Task<Weather> GetCurrentWeatherAsync(float latitude, float longitude, Units units)
    {
        throw new NotImplementedException();
    }

    public Task<List<Weather>> GetWeatherForecastAsync(float latitude, float longitude, Units units)
    {
        throw new NotImplementedException();
    }
}
