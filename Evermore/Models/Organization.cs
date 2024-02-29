namespace Evermore.Models;

public class Organization
{
    public OrganizationId Id { get; set; }
    public LocationId LocationId { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; } = "";
}
public readonly record struct OrganizationId(Guid Value);
