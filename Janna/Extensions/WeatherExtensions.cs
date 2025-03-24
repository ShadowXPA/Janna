using Janna.Weather;

namespace Janna.Extensions;

public static class WeatherExtensions
{
    public static string ToTemperatureString(this float temperature, Units units) => units switch
    {
        Units.Standard => $"{temperature} ºK",
        Units.Metric => $"{temperature} ºC",
        Units.Imperial => $"{temperature} ºF",
        _ => ""
    };

    public static string ToWindSpeedString(this float windSpeed, Units units) => units switch
    {
        Units.Standard => $"{windSpeed} m/s",
        Units.Metric => $"{windSpeed} m/s",
        Units.Imperial => $"{windSpeed} mph",
        _ => ""
    };

    public static string ToVisibilityString(this int visibility) => visibility == 10000 ? "Unlimited" : $"{visibility} m";

    public static string ToWindDirectionString(this int windDirection) => windDirection switch
    {
        >= 0 and < 23 or >= 338 and < 360 => ":arrow_down: South",
        >= 23 and < 68 => ":arrow_lower_right: South East",
        >= 68 and < 113 => ":arrow_right: East",
        >= 113 and < 158 => ":arrow_upper_right: North East",
        >= 158 and < 203 => ":arrow_up: North",
        >= 203 and < 248 => ":arrow_upper_left: North West",
        >= 248 and < 293 => ":arrow_left: West",
        >= 293 and < 338 => ":arrow_lower_left: Sounth West",
        _ => ""
    };

    public static string ToWindDirectionEmoji(this int windDirection) => windDirection switch
    {
        >= 0 and < 23 or >= 338 and < 360 => ":arrow_down:",
        >= 23 and < 68 => ":arrow_lower_right:",
        >= 68 and < 113 => ":arrow_right:",
        >= 113 and < 158 => ":arrow_upper_right:",
        >= 158 and < 203 => ":arrow_up:",
        >= 203 and < 248 => ":arrow_upper_left:",
        >= 248 and < 293 => ":arrow_left:",
        >= 293 and < 338 => ":arrow_lower_left:",
        _ => ""
    };
}
