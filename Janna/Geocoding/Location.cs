namespace Janna.Geocoding;

public class Location
{
    public required string Name { get; set; }
    public required string CountryCode { get; set; }
    public required Coordinates Coordinates { get; set; }
}
