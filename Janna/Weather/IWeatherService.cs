namespace Janna;

public interface IWeatherService
{
    Task<Weather> GetCurrentWeatherAsync(float latitude, float longitude, Units units);
    Task<List<Weather>> GetWeatherForecastAsync(float latitude, float longitude, Units units);
}
