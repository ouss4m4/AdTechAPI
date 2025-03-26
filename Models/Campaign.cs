public class Campaign
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string AdPreferences { get; set; } // or use a JsonDocument if needed
}