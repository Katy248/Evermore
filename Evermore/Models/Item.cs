namespace Evermore.Models
{
    public class Item
    {
        public int Id { get; set; }
        public CharacterId CharacterId { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; } = "";
        public string? Description { get; set; } = "";
        public string? Damage { get; set; } = "";
        public bool IsUsed { get; set; } = false;
        public int? Amount { get; set; } = 0;
        public int? Weight { get; set; } = 0;
        public int? Price { get; set; } = 0;
    }
}
