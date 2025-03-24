using DSharpPlus.Entities;
using Janna.Utils;

namespace Janna.Extensions;

public static class DateTimeExtensions
{
    public static string ToIso8601String(this DateTimeOffset dateTimeOffset) => dateTimeOffset.ToString(Constants.DATE_TIME_ISO_8601_FORMAT);
    public static string ToShortTime(this DateTimeOffset dateTimeOffset) => dateTimeOffset.ToString(Constants.TIME_SHORT_FORMAT);
    public static string ToLongTime(this DateTimeOffset dateTimeOffset) => dateTimeOffset.ToString(Constants.TIME_LONG_FORMAT);
    public static DiscordColor ToColor(this DayOfWeek dayOfWeek) => dayOfWeek switch
    {
        DayOfWeek.Sunday => DiscordColor.Red,
        DayOfWeek.Monday => DiscordColor.Green,
        DayOfWeek.Tuesday => DiscordColor.Blue,
        DayOfWeek.Wednesday => DiscordColor.Purple,
        DayOfWeek.Thursday => DiscordColor.Teal,
        DayOfWeek.Friday => DiscordColor.Gold,
        DayOfWeek.Saturday => DiscordColor.Black,
        _ => DiscordColor.White
    };
}
