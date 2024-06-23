namespace Api.Data.Models;

public class Attraction
{
    public int AttractionId { get; set; }
    public required string Name { get; set; }
    public required string ParkName { get; set; }
}