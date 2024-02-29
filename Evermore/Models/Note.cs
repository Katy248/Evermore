namespace Evermore.Models;

public class Note
{
    public NoteId Id { get; set; }
    public CampaignId CampaignId { get; set; }
    public string Title { get; set; }
    public string NoteText { get; set; }
}
public readonly record struct NoteId(Guid Value);
