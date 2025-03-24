using Janna.Weather;

namespace Janna.Utils;

public static class Constants
{
    public static ulong? GetDiscordGuildId() => null;
    public const string DISCORD_BOT_TOKEN = "DISCORD_BOT_TOKEN";
    public const string OPEN_WEATHER_MAP_TOKEN = "OPEN_WEATHER_MAP_TOKEN";
    public const string DATE_TIME_ISO_8601_FORMAT = "yyyy-MM-ddTHH:mm:sszzz";
    public const string TIME_SHORT_FORMAT = "HH:mm";
    public const string TIME_LONG_FORMAT = "HH:mm:ss";

    public static string GetEnvironmentVariableOrThrow(string variable) => Environment.GetEnvironmentVariable(variable) ?? throw new Exception($"Environment variable '{variable}' is not set!");

    public static class Endpoint
    {
        public static string GetGeocodingEndpoint(string query, int limit, string appId) => $"https://api.openweathermap.org/geo/1.0/direct?q={query}&limit={limit}&appid={appId}";
        public static string GetCurrentWeatherEndpoint(float lat, float lon, Units units, string appId) => $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units={units}&appid={appId}";
        public static string GetWeatherForecastEndpoint(float lat, float lon, Units units, string appId) => $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&units={units}&appid={appId}";
        public static string GetWeatherIconEndpoint(string icon) => $"https://openweathermap.org/img/wn/{icon}@2x.png";
        public static string GetCountryFlagEndpoint(string countryCode) => $"https://flagsapi.com/{countryCode}/flat/64.png";
    }
}
