namespace Evermore.Models
{
    public class Character
    {
        public CharacterId Id { get; set; }
        public CampaignId CampaignId { get; set; }
        public Characteristics Characteristics { get; set; } = new();
        public SkillsProficiency Skills { get; set; } = new();
        public string? Name { get; set; }
        public string? PlayerName { get; set; }
        public string? Class { get; set; }
        public string? Race { get; set; }
        public int Level { get; set; } = 0;
        public int Experience { get; set; } = 0;
        public int MaxHp { get; set; } = 0;
        public int CurrentHp { get; set; } = 0;
        public int TemporaryHp { get; set; } = 0;
        public int MaxTemporaryHp { get; set; } = 0;
        public string? HpDice { get; set; }
        public int ArmorClass { get; set; } = 0;
        public int SpellDifficulty { get; set; } = 0;
        public int Movement { get; set; } = 0;
        public bool Inspiration { get; set; } = false;
        public string? Size { get; set; }
        public int Vision { get; set; } = 0;
        public string? DomainsAndLanguages { get; set; }
        public string? Appearance { get; set; }
        public string? CharacterTraits { get; set; }
        public string? Ideals { get; set; }
        public string? Affection { get; set; }
        public string? Weaknesses { get; set; }
    }
    public readonly record struct CharacterId(Guid Value);
}
