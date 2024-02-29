namespace Evermore.Models;
public class Quest
{
    public QuestId Id { get; set; }
    public LocationId LocationId { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; } = "";
}
public readonly record struct QuestId(Guid Value);

