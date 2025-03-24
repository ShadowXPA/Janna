namespace Janna.Weather;

public class Forecast
{
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
    public int Percipitation { get; set; }
    public DateTimeOffset DateTime { get; set; }
}
