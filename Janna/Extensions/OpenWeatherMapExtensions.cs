using Janna.Geocoding;
using Janna.OpenWeatherMap;
using Janna.Weather;

namespace Janna.Extensions;

public static class OpenWeatherMapExtensions
{
    public static Location ToLocation(this GeocodingResponse response)
    {
        return new()
        {
            Name = $"{response.Name}{(response.State is null ? "" : $", {response.State}")}, {response.Country}",
            CountryCode = response.Country,
            Coordinates = new()
            {
                Latitude = response.Lat,
                Longitude = response.Lon
            }
        };
    }

    public static CurrentWeather ToCurrentWeather(this CurrentWeatherResponse response, Location location, Units units)
    {
        var timeOffset = TimeSpan.FromSeconds(response.Timezone);
        var weather = response.Weather.First();

        return new()
        {
            Location = location,
            Units = units,
            Condition = weather.Main,
            Description = weather.Description,
            Icon = weather.Icon,
            Temperature = response.Main.Temp,
            FeelsLike = response.Main.FeelsLike,
            PressureSeaLevel = response.Main.SeaLevel,
            PressureGroundLevel = response.Main.GrndLevel,
            Humidity = response.Main.Humidity,
            Cloudiness = response.Clouds.All,
            WindSpeed = response.Wind.Speed,
            WindDirection = response.Wind.Deg,
            Visibility = response.Visibility,
            Sunrise = DateTimeOffset.FromUnixTimeSeconds(response.Sys.Sunrise).ToOffset(timeOffset),
            Sunset = DateTimeOffset.FromUnixTimeSeconds(response.Sys.Sunset).ToOffset(timeOffset),
            DateTime = DateTimeOffset.FromUnixTimeSeconds(response.Dt).ToOffset(timeOffset)
        };
    }

    public static WeatherForecast ToWeatherForecast(this WeatherForecastResponse response, Location location, Units units)
    {
        var timeOffset = TimeSpan.FromSeconds(response.City.Timezone);
        var forecast = new List<Forecast>(response.List.Count);

        response.List.ForEach(f =>
        {
            var weather = f.Weather.First();

            forecast.Add(new()
            {
                Condition = weather.Main,
                Description = weather.Description,
                Icon = weather.Icon,
                Temperature = f.Main.Temp,
                FeelsLike = f.Main.FeelsLike,
                PressureSeaLevel = f.Main.SeaLevel,
                PressureGroundLevel = f.Main.GrndLevel,
                Humidity = f.Main.Humidity,
                Cloudiness = f.Clouds.All,
                WindSpeed = f.Wind.Speed,
                WindDirection = f.Wind.Deg,
                Visibility = f.Visibility,
                Percipitation = (int)f.Pop * 100,
                DateTime = DateTimeOffset.FromUnixTimeSeconds(f.Dt).ToOffset(timeOffset)
            });
        });

        return new()
        {
            Location = location,
            Units = units,
            Forecast = forecast,
            Timezone = response.City.Timezone
        };
    }
}
