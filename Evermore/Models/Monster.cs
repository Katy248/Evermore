namespace Evermore.Models;
public class Monster
{
    public MonsterId Id { get; set; }
    public LocationId LocationId { get; set; }
    public string Name { get; set; } = "";
    public string? Size { get; set; } = "";
    public string? Type { get; set; } = "";
    public string? Outlook { get; set; } = "";
    public string? Description { get; set; } = "";
}
public readonly record struct MonsterId(Guid Value);

