namespace Evermore.Models;
public class Ability
{
    public AbilityId Id { get; set; }
    public CharacterId CharacterId { get; set; }
    public string? Name { get; set; } = "";
    public string? Type { get; set; } = "";
    public string? Description { get; set; } = "";
    public string? Activation { get; set; } = "";
}


public readonly record struct AbilityId(Guid Value);
