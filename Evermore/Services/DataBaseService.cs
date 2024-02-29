using Evermore.Data;
using Evermore.Models;
using System.Data;

namespace Evermore.Services;
public class DataBaseService
{
    private readonly DatabaseContext _context;

    public DataBaseService(DatabaseContext context)
    {
        _context = context;
    }
    public bool EmailExists(string email) =>
        _context.Users.Any(u => u.Email == email);

    public bool AddUser(User user)
    {
        if (!EmailExists(user.Email))
            return false;

        _context.Users.Add(user);
        _context.SaveChanges();
        return true;
    }

    public User? GetUser(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public IEnumerable<Campaign> GetAllCampaigns(UserId id) =>
        _context.CampaignDatas.Where(c => c.UserId == id).ToArray();

    public void AddNewCampaign(Campaign campaignData)
    {
        _context.CampaignDatas.Add(campaignData);
        _context.SaveChanges();
    }

    public IEnumerable<Item> GetItems(CharacterId characterId) =>
        _context.Items.Where(i => i.CharacterId == characterId).ToArray();

    public void AddItem(Item item)
    {
        _context.Items.Add(item);
        _context.SaveChanges();
    }

    public void UpdateItem(Item item)
    {
        _context.Items.Update(item);
        _context.SaveChanges();
    }
    public IEnumerable<Ability> GetAbilities(CharacterId characterId) =>
        _context.Abilities.Where(a => a.CharacterId == characterId).ToArray();

    public void AddAbility(Ability ability)
    {
        _context.Abilities.Add(ability);
        _context.SaveChanges();
    }

    public void UpdateAbility(Ability ability)
    {
        _context.Abilities.Update(ability);
        _context.SaveChanges();
    }
    public IEnumerable<Spell> GetSpells(CharacterId characterId) =>
        _context.Spells.Where(s => s.CharacterId == characterId);

    public void AddSpell(Spell spell)
    {
        _context.Spells.Add(spell);
        _context.SaveChanges();
    }

    public void UpdateSpell(Spell spell)
    {
        _context.Spells.Update(spell);
        _context.SaveChanges();
    }

    public IEnumerable<Character> GetAllCharacters(CampaignId campaignId) =>
        _context.CharacterDatas.Where(c => c.CampaignId == campaignId).ToArray();
    public Character? GetCharacterById(CharacterId id) =>
        _context.CharacterDatas.FirstOrDefault(c => c.Id == id);

    public void AddCharacter(Character character)
    {
        _context.CharacterDatas.Add(character);
        _context.SaveChanges();
    }

    public void UpdateCharacter(Character character)
    {
        _context.CharacterDatas.Update(character);
        _context.SaveChanges();
    }

    public void DeleteCharacter(CharacterId characterId)
    {
        var character = _context.CharacterDatas.FirstOrDefault(c => c.Id == characterId);
        _context.CharacterDatas.Remove(character);
        _context.SaveChanges();
    }

    public IEnumerable<Location> GetAllLocations(CampaignId campaignId) =>
        _context.Locations.Where(l => l.CampaignId == campaignId).ToArray();

    public void AddLocation(Location location)
    {
        _context.Locations.Add(location);
        _context.SaveChanges();
    }

    public void UpdateLocation(Location location)
    {
        _context.Locations.Update(location);
        _context.SaveChanges();
    }

    public void DeleteLocation(LocationId locationId)
    {
        var location = _context.Locations.FirstOrDefault(l => l.Id == locationId);
        _context.Locations.Remove(location);
        _context.SaveChanges();
    }

    public IEnumerable<Organization> GetAllOrganizations(LocationId locationId) =>
        _context.Organizations.Where(o => o.LocationId == locationId).ToArray();
    public void AddOrganization(Organization organization)
    {
        _context.Organizations.Add(organization);
        _context.SaveChanges();
    }

    public void UpdateOrganization(Organization organization)
    {
        _context.Organizations.Update(organization);
        _context.SaveChanges();
    }

    public void DeleteOrganization(OrganizationId organizationId)
    {
        var organization = _context.Organizations.FirstOrDefault(o => o.Id == organizationId);
        _context.Organizations.Remove(organization);
        _context.SaveChanges();
    }

    public IEnumerable<Quest> GetAllQuests(LocationId locationId)
    {
        return _context.Quests.Where(q => q.LocationId == locationId).ToArray();
    }

    public void AddQuest(Quest quest)
    {
        _context.Quests.Add(quest);
        _context.SaveChanges();
    }

    public void UpdateQuest(Quest quest)
    {
        _context.Quests.Update(quest);
        _context.SaveChanges();
    }

    public void DeleteQuest(QuestId questId)
    {
        var quest = _context.Quests.FirstOrDefault(q => q.Id == questId);
        _context.Quests.Remove(quest);
        _context.SaveChanges();
    }

    public IEnumerable<Monster> GetAllMonsters(LocationId locationId) =>
        _context.Monsters.Where(m => m.LocationId == locationId).ToArray();
    public void AddMonster(Monster monster)
    {
        _context.Monsters.Add(monster);
        _context.SaveChanges();
    }

    public void UpdateMonster(Monster monster)
    {
        _context.Monsters.Update(monster);
        _context.SaveChanges();
    }

    public void DeleteMonster(MonsterId id)
    {
        var monster = _context.Monsters.FirstOrDefault(m => m.Id == id);
        _context.Monsters.Remove(monster);
        _context.SaveChanges();
    }
    public IEnumerable<Note> GetNotes(CampaignId campaignId) =>
        _context.Notes.Where(n => n.CampaignId == campaignId).ToArray();

    public void AddNewNote(Note note)
    {
        _context.Notes.Add(note);
        _context.SaveChanges();
    }

    public void UpdateNote(Note note)
    {
        _context.Notes.Update(note);
        _context.SaveChanges();
    }

    public void DeleteNote(NoteId noteId)
    {
        var note = _context.Notes.FirstOrDefault(n => n.Id == noteId);
        _context.Notes.Add(note);
        _context.SaveChanges();
    }
}
