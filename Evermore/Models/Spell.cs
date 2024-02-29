namespace Evermore.Models;

public class Spell
{
    public SpellId Id { get; set; }
    public CharacterId CharacterId { get; set; }
    public string? Name { get; set; } = "";
    public int? SpellLevel { get; set; } = 0;
    public string? Description { get; set; } = "";
    public string? ActivationType { get; set; } = "";
    public string? Target { get; set; } = "";
}
public readonly record struct SpellId(Guid Value);
