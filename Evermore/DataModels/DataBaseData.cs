namespace Evermore.DataModels
{
    public class UserAccountData
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class CampaignData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }

    public class dbCharacterData
    {
        public int CharacterId { get; set; }
        public int CampaignId { get; set; }
        public string? Name { get; set; }
        public string? PlayerName { get; set; } = "";
        public string? Class { get; set; } = "";
        public string? Race { get; set; } = "";
        public int? Level { get; set; } = 0;
        public int? Experience { get; set; } = 0;
        public Characteristics Characteristics { get; set; } = new Characteristics(0, false, 0, false, 0, false, 0, false, 0, false, 0, false);
        public int? MaxHp { get; set; } = 0;
        public int? CurrentHp { get; set; } = 0;    
        public int? TemporaryHp { get; set; } = 0;
        public int? MaxTemporaryHp { get; set; } = 0;
        public string? HpDice { get; set; } = "";
        public int? ArmorClass { get; set; } = 0;
        public int? SpellDifficulty { get; set; } = 0;
        public int? Movement { get; set; } = 0;
        public SkillsProficiency Skills { get; set; } = new SkillsProficiency(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
        public bool? Inspiration { get; set; } = false;
        public string? Size { get; set; } = "";
        public int? Vision { get; set; } = 0;
        public string? DomainsAndLanguages { get; set; } = "";
        public string? Appearance { get; set; } = "";
        public string? CharacterTraits { get; set; } = "";
        public string? Ideals { get; set; } = "";
        public string? Affection { get; set; } = "";
        public string? Weaknesses { get; set; } = "";
    }
    public class Characteristics
    {
        public int StrengthValue { get; set; } = 20;
        public bool StrengthProficient { get; set; } = false;

        public int DexterityValue { get; set; } = 0;
        public bool DexterityProficient { get; set; } = false;

        public int ConstitutionValue { get; set; } = 0;
        public bool ConstitutionProficient { get; set; } = false;

        public int IntelligenceValue { get; set; } = 0;
        public bool IntelligenceProficient { get; set; } = false;

        public int WisdomValue { get; set; } = 0;
        public bool WisdomProficient { get; set; } = false;

        public int CharismaValue { get; set; } = 0;
        public bool CharismaProficient { get; set; } = false;

        public Characteristics(int strengthValue, bool strengthProficient, int dexterityValue, bool dexterityProficient, int constitutionValue, bool constitutionProficient, int intelligenceValue, bool intelligenceProficient, int wisdomValue, bool wisdomProficient, int charismaValue, bool charismaProficient)
        {
            StrengthValue = strengthValue;
            StrengthProficient = strengthProficient;
            DexterityValue = dexterityValue;
            DexterityProficient = dexterityProficient;
            ConstitutionValue = constitutionValue;
            ConstitutionProficient = constitutionProficient;
            IntelligenceValue = intelligenceValue;
            IntelligenceProficient = intelligenceProficient;
            WisdomValue = wisdomValue;
            WisdomProficient = wisdomProficient;
            CharismaValue = charismaValue;
            CharismaProficient = charismaProficient;
        }
    }
    public class SkillsProficiency
    {
        public bool Athletics { get; set; } = false;
        public bool Acrobatics { get; set; } = false;
        public bool SleightofHand { get; set; } = false;
        public bool Stealth { get; set; } = false;
        public bool Arcana { get; set; } = false;
        public bool History { get; set; } = false;
        public bool Investigation { get; set; } = false;
        public bool Nature { get; set; } = false;
        public bool Religion { get; set; } = false;
        public bool AnimalHandling { get; set; } = false;
        public bool Insight { get; set; } = false;
        public bool Medicine { get; set; } = false;
        public bool Perception { get; set; } = false;
        public bool Survival { get; set; } = false;
        public bool Deception { get; set; } = false;
        public bool Intimidation { get; set; } = false;
        public bool Performance { get; set; } = false;
        public bool Persuasion { get; set; } = false;

        public SkillsProficiency(bool athletics, bool acrobatics, bool sleightofHand, bool stealth, bool arcana, bool history, bool investigation, bool nature, bool religion, bool animalHandling, bool insight, bool medicine, bool perception, bool survival, bool deception, bool intimidation, bool performance, bool persuasion)
        {
            Athletics = athletics;
            Acrobatics = acrobatics;
            SleightofHand = sleightofHand;
            Stealth = stealth;
            Arcana = arcana;
            History = history;
            Investigation = investigation;
            Nature = nature;
            Religion = religion;
            AnimalHandling = animalHandling;
            Insight = insight;
            Medicine = medicine;
            Perception = perception;
            Survival = survival;
            Deception = deception;
            Intimidation = intimidation;
            Performance = performance;
            Persuasion = persuasion;
        }
    }
    public class Item
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string? Name { get; set; } = "";
        public string? Type { get; set; } = "";
        public string? Description { get; set; } = "";
        public string? Damage { get; set; } = "";
        public bool IsUsed { get; set; } = false;
        public int? Amount { get; set; } = 0;
        public int? Weight { get; set; } = 0;
        public int? Price { get; set; } = 0;
    }
    public class Ability
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string? Name { get; set; } = "";
        public string? Type { get; set; } = "";
        public string? Description { get; set; } = "";
        public string? Activation { get; set; } = "";
    }
    public class Spell
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string? Name { get; set; } = "";
        public int? SpellLevel { get; set; } = 0;
        public string? Description { get; set; } = "";
        public string? ActivationType { get; set; } = "";
        public string? Target { get; set; } = "";
    }
     
    public class Location
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string? Name { get; set; }
    }
    public class Organization
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; } = "";
    }
    public class Quest
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; } = "";
    }

    public class Monster
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; } = "";
        public string? Size { get; set; } = "";
        public string? Type { get; set; } = "";
        public string? Outlook { get; set; } = "";
        public string? Description { get; set; } = "";
    }

    public class Note
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string Title { get; set; }
        public string? NoteText { get; set; } = "";
    }
}
