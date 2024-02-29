namespace Evermore.Models;

public class Location
{
    public LocationId Id { get; set; }
    public CampaignId CampaignId { get; set; }
    public string? Name { get; set; }
}
public readonly record struct LocationId(Guid Value);
