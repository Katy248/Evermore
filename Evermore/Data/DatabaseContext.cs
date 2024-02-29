using Evermore.Models;
using Microsoft.EntityFrameworkCore;

namespace Evermore.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    public DbSet<Ability> Abilities { get; set; }
    public DbSet<Campaign> CampaignDatas { get; set; }
    public DbSet<Character> CharacterDatas { get; set; }
    public DbSet<Characteristics> Characteristics { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Monster> Monsters { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Quest> Quests { get; set; }
    public DbSet<SkillsProficiency> SkillsProficiencies { get; set; }
    public DbSet<Spell> Spells { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }

}
