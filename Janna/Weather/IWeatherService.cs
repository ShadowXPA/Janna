namespace Janna.Weather;

public interface IWeatherService
{
    Task<CurrentWeather?> GetCurrentWeatherAsync(string query, Units units);
    Task<WeatherForecast?> GetWeatherForecastAsync(string query, Units units);
}
