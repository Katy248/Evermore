namespace Evermore.Models;
public class Campaign
{
    public CampaignId Id { get; set; }
    public UserId UserId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}
public readonly record struct CampaignId(Guid Value);

