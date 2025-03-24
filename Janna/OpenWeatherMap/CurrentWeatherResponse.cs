namespace Janna.OpenWeatherMap;

public class CurrentWeatherResponse
{
    public required List<W> Weather { get; set; }
    public required M Main { get; set; }
    public int Visibility { get; set; }
    public required Wi Wind { get; set; }
    public required C Clouds { get; set; }
    public long Dt { get; set; }
    public required S Sys { get; set; }
    public long Timezone { get; set; }

    public class W
    {
        public required string Main { get; set; }
        public required string Description { get; set; }
        public required string Icon { get; set; }
    }

    public class M
    {
        public float Temp { get; set; }
        public float FeelsLike { get; set; }
        public int SeaLevel { get; set; }
        public int GrndLevel { get; set; }
        public int Humidity { get; set; }
    }

    public class Wi
    {
        public float Speed { get; set; }
        public int Deg { get; set; }
    }

    public class C
    {
        public int All { get; set; }
    }

    public class S
    {
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }
}
