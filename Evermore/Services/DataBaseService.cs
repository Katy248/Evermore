using Evermore.DataModels;
using Evermore.Pages;
using Npgsql;
using System.Data;

namespace Evermore.Services
{
    public class DataBaseService
    {        
        private readonly string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id =postgres;Password=123123";

        #region User

        public bool IsEmailNew(string email)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT COUNT(*) FROM Users " +
                    "WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    var result = (long)command.ExecuteScalar();

                    return result == 0;
                }
            }
        }

        public bool AddUser(UserAccountData user)
        {
            if (!IsEmailNew(user.Email))
                return false;

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO Users (Email, Username, Password) " +
                "VALUES (@Email, @Username, @Password)", connection);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", user.Password);

            command.ExecuteNonQuery();
            return true;
        }

        public UserAccountData GetUser(string email, string password)
        {
            var userData = new UserAccountData();

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("SELECT * FROM Users " +
                "WHERE Email = @Email AND Password = @Password", connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);

            using var reader = command.ExecuteReader();
            while(reader.Read())
            {
                userData = new UserAccountData {
                    UserId = reader.GetInt32("UserId"),
                    Username = reader.GetString("Username"),
                    Email = reader.GetString("Email"), 
                    Password = reader.GetString("Password")
                };
            }

            return userData;
        }

        #endregion

        #region Campaign

        public List<CampaignData> GetAllCampaigns(int UserId)
        {
            List<CampaignData> campaigns = new List<CampaignData>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using var command = new NpgsqlCommand("SELECT * FROM Campaigns WHERE UserId = @UserId", connection);
                command.Parameters.AddWithValue("@UserId", UserId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var campaignData = new CampaignData
                    {
                        Id = reader.GetInt32("CampaignId"),
                        Title = reader.GetString("Title"),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description")
                    };
                    campaigns.Add(campaignData);
                }
            }
            return campaigns;
        }

        public void AddNewCampaign(CampaignData campaignData)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO Campaigns (UserID, Title, Description) VALUES (@UserId, @Title, @Description)", connection);
            command.Parameters.AddWithValue("@UserId", campaignData.UserId);
            command.Parameters.AddWithValue("@Title", campaignData.Title);
            command.Parameters.AddWithValue("@Description", campaignData.Description ?? "");;

            command.ExecuteNonQuery();
        }

        #endregion

        #region Character items
        public List<Item> GetItems(int characterId)
        {
            List<Item> items = new List<Item>();

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using (var command = new NpgsqlCommand("SELECT * FROM Items WHERE CharacterId = @CharacterId", connection))
            {
                command.Parameters.AddWithValue("@CharacterId", characterId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = new Item
                    {
                        Id = reader.GetInt32("Id"),
                        CharacterId = reader.GetInt32("CharacterId"),
                        Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString("Name"),
                        Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? null : reader.GetString("Type"),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                        Damage = reader.IsDBNull(reader.GetOrdinal("Damage")) ? null : reader.GetString("Damage"),
                        IsUsed = reader.GetBoolean("IsUsed"),
                        Amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? null : reader.GetInt32("Amount"),
                        Weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? null : reader.GetInt32("Weight"),
                        Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? null : reader.GetInt32("Price")
                    };

                    items.Add(item);
                }
            }

            return items;
        }

        public void AddItem(Item item)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO Items (CharacterId, Name, Type, Description, Damage, IsUsed, Amount, Weight, Price) " +
                "VALUES (@CharacterId, @Name, @Type, @Description, @Damage, @IsUsed, @Amount, @Weight, @Price)", connection);
            command.Parameters.AddWithValue("@CharacterId", item.CharacterId);
            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Type", item.Type);
            command.Parameters.AddWithValue("@Description", item.Description);
            command.Parameters.AddWithValue("@Damage", item.Damage);
            command.Parameters.AddWithValue("@IsUsed", item.IsUsed);
            command.Parameters.AddWithValue("@Amount", item.Amount);
            command.Parameters.AddWithValue("@Weight", item.Weight);
            command.Parameters.AddWithValue("@Price", item.Price);

            command.ExecuteNonQuery();
        }

        public void UpdateItem(Item item)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("UPDATE Items SET Name = @Name, Type = @Type, Description = @Description, " +
                "Damage = @Damage, IsUsed = @IsUsed, Amount = @Amount, Weight = @Weight, Price = @Price " +
                "WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", item.Id);
            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Type", item.Type);
            command.Parameters.AddWithValue("@Description", item.Description);
            command.Parameters.AddWithValue("@Damage", item.Damage);
            command.Parameters.AddWithValue("@IsUsed", item.IsUsed);
            command.Parameters.AddWithValue("@Amount", item.Amount);
            command.Parameters.AddWithValue("@Weight", item.Weight);
            command.Parameters.AddWithValue("@Price", item.Price);

            command.ExecuteNonQuery();
        }

        #endregion

        #region Character abilities
        public List<Ability> GetAbilities(int characterId)
        {
            List<Ability> abilities = new List<Ability>();

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using (var command = new NpgsqlCommand("SELECT * FROM Abilities WHERE CharacterId = @CharacterId", connection))
            {
                command.Parameters.AddWithValue("@CharacterId", characterId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ability = new Ability
                        {
                            Id = reader.GetInt32("Id"),
                            CharacterId = reader.GetInt32("CharacterId"),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString("Name"),
                            Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? null : reader.GetString("Type"),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                            Activation = reader.IsDBNull(reader.GetOrdinal("Activation")) ? null : reader.GetString("Activation")
                        };

                        abilities.Add(ability);
                    }
                }
            }

            return abilities;
        }

        public void AddAbility(Ability ability)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO Abilities (CharacterId, Name, Type, Description, Activation) " +
                "VALUES (@CharacterId, @Name, @Type, @Description, @Activation)", connection);
            command.Parameters.AddWithValue("@CharacterId", ability.CharacterId);
            command.Parameters.AddWithValue("@Name", ability.Name);
            command.Parameters.AddWithValue("@Type", ability.Type);
            command.Parameters.AddWithValue("@Description", ability.Description);
            command.Parameters.AddWithValue("@Activation", ability.Activation);

            command.ExecuteNonQuery();
        }

        public void UpdateAbility(Ability ability)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("UPDATE Abilities SET Name = @Name, Type = @Type, Description = @Description, " +
                "Activation = @Activation WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", ability.Id);
            command.Parameters.AddWithValue("@Name", ability.Name);
            command.Parameters.AddWithValue("@Type", ability.Type);
            command.Parameters.AddWithValue("@Description", ability.Description);
            command.Parameters.AddWithValue("@Activation", ability.Activation);

            command.ExecuteNonQuery();
        }


        #endregion

        #region Character spells
        public List<Spell> GetSpells(int characterId)
        {
            List<Spell> spells = new List<Spell>();

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using (var command = new NpgsqlCommand("SELECT * FROM Spells WHERE CharacterId = @CharacterId", connection))
            {
                command.Parameters.AddWithValue("@CharacterId", characterId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var spell = new Spell
                        {
                            Id = reader.GetInt32("Id"),
                            CharacterId = reader.GetInt32("CharacterId"),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString("Name"),
                            SpellLevel = reader.IsDBNull(reader.GetOrdinal("SpellLevel")) ? null : reader.GetInt32("SpellLevel"),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                            ActivationType = reader.IsDBNull(reader.GetOrdinal("ActivationType")) ? null : reader.GetString("ActivationType"),
                            Target = reader.IsDBNull(reader.GetOrdinal("Target")) ? null : reader.GetString("Target")
                        };

                        spells.Add(spell);
                    }
                }
            }

            return spells;
        }

        public void AddSpell(Spell spell)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO Spells (CharacterId, Name, SpellLevel, Description, ActivationType, Target) " +
                "VALUES (@CharacterId, @Name, @SpellLevel, @Description, @ActivationType, @Target)", connection);
            command.Parameters.AddWithValue("@CharacterId", spell.CharacterId);
            command.Parameters.AddWithValue("@Name", spell.Name);
            command.Parameters.AddWithValue("@SpellLevel", spell.SpellLevel);
            command.Parameters.AddWithValue("@Description", spell.Description);
            command.Parameters.AddWithValue("@ActivationType", spell.ActivationType);
            command.Parameters.AddWithValue("@Target", spell.Target);

            command.ExecuteNonQuery();
        }

        public void UpdateSpell(Spell spell)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using var command = new NpgsqlCommand("UPDATE Spells SET Name = @Name, SpellLevel = @SpellLevel, " +
                    "Description = @Description, ActivationType = @ActivationType, Target = @Target " +
                    "WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", spell.Id);
                command.Parameters.AddWithValue("@Name", spell.Name);
                command.Parameters.AddWithValue("@SpellLevel", spell.SpellLevel);
                command.Parameters.AddWithValue("@Description", spell.Description);
                command.Parameters.AddWithValue("@ActivationType", spell.ActivationType);
                command.Parameters.AddWithValue("@Target", spell.Target);

                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Character

        public List<dbCharacterData> GetAllCharacters(int CampaingId)
        {
            List<dbCharacterData> characters = new List<dbCharacterData>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using var command = new NpgsqlCommand("SELECT * FROM EvermoreCharacters WHERE CampaignId = @CampaignId", connection);
                command.Parameters.AddWithValue("@CampaignId", CampaingId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var character = new dbCharacterData
                    {
                        CharacterId = reader.GetInt32("CharacterId"),
                        CampaignId = reader.GetInt32("CampaignId"),
                        Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString("Name"),
                        PlayerName = reader.IsDBNull(reader.GetOrdinal("PlayerName")) ? null : reader.GetString("PlayerName"),
                        Class = reader.IsDBNull(reader.GetOrdinal("Class")) ? null : reader.GetString("Class"),
                        Race = reader.IsDBNull(reader.GetOrdinal("Race")) ? null : reader.GetString("Race"),
                        Level = reader.IsDBNull(reader.GetOrdinal("Level")) ? null : reader.GetInt32("Level"),
                        Experience = reader.IsDBNull(reader.GetOrdinal("Experience")) ? null : reader.GetInt32("Experience"),
                        Characteristics = new Characteristics(
                            reader.GetInt32("StrengthValue"),
                            reader.GetBoolean("StrengthProficient"),
                            reader.GetInt32("DexterityValue"),
                            reader.GetBoolean("DexterityProficient"),
                            reader.GetInt32("ConstitutionValue"),
                            reader.GetBoolean("ConstitutionProficient"),
                            reader.GetInt32("IntelligenceValue"),
                            reader.GetBoolean("IntelligenceProficient"),
                            reader.GetInt32("WisdomValue"),
                            reader.GetBoolean("WisdomProficient"),
                            reader.GetInt32("CharismaValue"),
                            reader.GetBoolean("CharismaProficient")
                        ),
                        MaxHp = reader.IsDBNull(reader.GetOrdinal("MaxHp")) ? null : reader.GetInt32("MaxHp"),
                        CurrentHp = reader.IsDBNull(reader.GetOrdinal("CurrentHp")) ? null : reader.GetInt32("CurrentHp"),
                        TemporaryHp = reader.IsDBNull(reader.GetOrdinal("TemporaryHp")) ? null : reader.GetInt32("TemporaryHp"),
                        MaxTemporaryHp = reader.IsDBNull(reader.GetOrdinal("MaxTemporaryHp")) ? null : reader.GetInt32("MaxTemporaryHp"),
                        HpDice = reader.IsDBNull(reader.GetOrdinal("HpDice")) ? null : reader.GetString("HpDice"),
                        ArmorClass = reader.IsDBNull(reader.GetOrdinal("ArmorClass")) ? null : reader.GetInt32("ArmorClass"),
                        SpellDifficulty = reader.IsDBNull(reader.GetOrdinal("SpellDifficulty")) ? null : reader.GetInt32("SpellDifficulty"),
                        Movement = reader.IsDBNull(reader.GetOrdinal("Movement")) ? null : reader.GetInt32("Movement"),
                        Skills = new SkillsProficiency
                        (
                            reader.GetBoolean("Athletics"),
                            reader.GetBoolean("Acrobatics"),
                            reader.GetBoolean("SleightofHand"),
                            reader.GetBoolean("Stealth"),
                            reader.GetBoolean("Arcana"),
                            reader.GetBoolean("History"),
                            reader.GetBoolean("Investigation"),
                            reader.GetBoolean("Nature"),
                            reader.GetBoolean("Religion"),
                            reader.GetBoolean("AnimalHandling"),
                            reader.GetBoolean("Insight"),
                            reader.GetBoolean("Medicine"),
                            reader.GetBoolean("Perception"),
                            reader.GetBoolean("Survival"),
                            reader.GetBoolean("Deception"),
                            reader.GetBoolean("Intimidation"),
                            reader.GetBoolean("Performance"),
                            reader.GetBoolean("Persuasion")
                        ),
                        Inspiration = reader.IsDBNull(reader.GetOrdinal("Inspiration")) ? null : reader.GetBoolean("Inspiration"),
                        Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? null : reader.GetString("Size"),
                        Vision = reader.IsDBNull(reader.GetOrdinal("Vision")) ? null : reader.GetInt32("Vision"),
                        DomainsAndLanguages = reader.IsDBNull(reader.GetOrdinal("DomainsAndLanguages")) ? null : reader.GetString("DomainsAndLanguages"),
                        /*
                        Equipment = GetItems(reader.GetInt32("CharacterId"), connection),
                        Abilities = GetAbilities(reader.GetInt32("CharacterId"), connection),
                        Spells = GetSpells(reader.GetInt32("CharacterId"), connection),
                        */
                        Appearance = reader.IsDBNull(reader.GetOrdinal("Appearance")) ? null : reader.GetString("Appearance"),
                        CharacterTraits = reader.IsDBNull(reader.GetOrdinal("CharacterTraits")) ? null : reader.GetString("CharacterTraits"),
                        Ideals = reader.IsDBNull(reader.GetOrdinal("Ideals")) ? null : reader.GetString("Ideals"),
                        Affection = reader.IsDBNull(reader.GetOrdinal("Affection")) ? null : reader.GetString("Affection"),
                        Weaknesses = reader.IsDBNull(reader.GetOrdinal("Weaknesses")) ? null : reader.GetString("Weaknesses")
                    };

                    characters.Add(character);
                }
            }

            return characters;
        }

        public dbCharacterData GetCharacterById(int CharacterId)
        {
            var character = new dbCharacterData();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using var command = new NpgsqlCommand("SELECT * FROM EvermoreCharacters WHERE CharacterId = @CharacterId", connection);
                command.Parameters.AddWithValue("CharacterId", CharacterId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    character = new dbCharacterData
                    {
                        CharacterId = reader.GetInt32("CharacterId"),
                        CampaignId = reader.GetInt32("CampaignId"),
                        Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString("Name"),
                        PlayerName = reader.IsDBNull(reader.GetOrdinal("PlayerName")) ? null : reader.GetString("PlayerName"),
                        Class = reader.IsDBNull(reader.GetOrdinal("Class")) ? null : reader.GetString("Class"),
                        Race = reader.IsDBNull(reader.GetOrdinal("Race")) ? null : reader.GetString("Race"),
                        Level = reader.IsDBNull(reader.GetOrdinal("Level")) ? null : reader.GetInt32("Level"),
                        Experience = reader.IsDBNull(reader.GetOrdinal("Experience")) ? null : reader.GetInt32("Experience"),
                        Characteristics = new Characteristics(
                            reader.GetInt32("StrengthValue"),
                            reader.GetBoolean("StrengthProficient"),
                            reader.GetInt32("DexterityValue"),
                            reader.GetBoolean("DexterityProficient"),
                            reader.GetInt32("ConstitutionValue"),
                            reader.GetBoolean("ConstitutionProficient"),
                            reader.GetInt32("IntelligenceValue"),
                            reader.GetBoolean("IntelligenceProficient"),
                            reader.GetInt32("WisdomValue"),
                            reader.GetBoolean("WisdomProficient"),
                            reader.GetInt32("CharismaValue"),
                            reader.GetBoolean("CharismaProficient")
                        ),
                        MaxHp = reader.IsDBNull(reader.GetOrdinal("MaxHp")) ? null : reader.GetInt32("MaxHp"),
                        CurrentHp = reader.IsDBNull(reader.GetOrdinal("CurrentHp")) ? null : reader.GetInt32("CurrentHp"),
                        TemporaryHp = reader.IsDBNull(reader.GetOrdinal("TemporaryHp")) ? null : reader.GetInt32("TemporaryHp"),
                        MaxTemporaryHp = reader.IsDBNull(reader.GetOrdinal("MaxTemporaryHp")) ? null : reader.GetInt32("MaxTemporaryHp"),
                        HpDice = reader.IsDBNull(reader.GetOrdinal("HpDice")) ? null : reader.GetString("HpDice"),
                        ArmorClass = reader.IsDBNull(reader.GetOrdinal("ArmorClass")) ? null : reader.GetInt32("ArmorClass"),
                        SpellDifficulty = reader.IsDBNull(reader.GetOrdinal("SpellDifficulty")) ? null : reader.GetInt32("SpellDifficulty"),
                        Movement = reader.IsDBNull(reader.GetOrdinal("Movement")) ? null : reader.GetInt32("Movement"),
                        Skills = new SkillsProficiency
                        (
                            reader.GetBoolean("Athletics"),
                            reader.GetBoolean("Acrobatics"),
                            reader.GetBoolean("SleightofHand"),
                            reader.GetBoolean("Stealth"),
                            reader.GetBoolean("Arcana"),
                            reader.GetBoolean("History"),
                            reader.GetBoolean("Investigation"),
                            reader.GetBoolean("Nature"),
                            reader.GetBoolean("Religion"),
                            reader.GetBoolean("AnimalHandling"),
                            reader.GetBoolean("Insight"),
                            reader.GetBoolean("Medicine"),
                            reader.GetBoolean("Perception"),
                            reader.GetBoolean("Survival"),
                            reader.GetBoolean("Deception"),
                            reader.GetBoolean("Intimidation"),
                            reader.GetBoolean("Performance"),
                            reader.GetBoolean("Persuasion")
                        ),
                        Inspiration = reader.IsDBNull(reader.GetOrdinal("Inspiration")) ? null : reader.GetBoolean("Inspiration"),
                        Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? null : reader.GetString("Size"),
                        Vision = reader.IsDBNull(reader.GetOrdinal("Vision")) ? null : reader.GetInt32("Vision"),
                        DomainsAndLanguages = reader.IsDBNull(reader.GetOrdinal("DomainsAndLanguages")) ? null : reader.GetString("DomainsAndLanguages"),
                        /*
                        Equipment = GetItems(reader.GetInt32("CharacterId"), connection),
                        Abilities = GetAbilities(reader.GetInt32("CharacterId"), connection),
                        Spells = GetSpells(reader.GetInt32("CharacterId"), connection),
                        */
                        Appearance = reader.IsDBNull(reader.GetOrdinal("Appearance")) ? null : reader.GetString("Appearance"),
                        CharacterTraits = reader.IsDBNull(reader.GetOrdinal("CharacterTraits")) ? null : reader.GetString("CharacterTraits"),
                        Ideals = reader.IsDBNull(reader.GetOrdinal("Ideals")) ? null : reader.GetString("Ideals"),
                        Affection = reader.IsDBNull(reader.GetOrdinal("Affection")) ? null : reader.GetString("Affection"),
                        Weaknesses = reader.IsDBNull(reader.GetOrdinal("Weaknesses")) ? null : reader.GetString("Weaknesses")
                    };
                }
            }

            return character;
        }

        public void AddNewCharacter(dbCharacterData character)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO EvermoreCharacters (CampaignId, Name) VALUES (@CampaignId, @Name)", connection);
            command.Parameters.AddWithValue("@CampaignId", character.CampaignId);
            command.Parameters.AddWithValue("@Name", character.Name);

            command.ExecuteNonQuery();
        }

        public void AddCharacter(dbCharacterData character)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO EvermoreCharacters (CampaignId, Name, PlayerName, Class, Race, Level, Experience, " +
                "StrengthValue, StrengthProficient, DexterityValue, DexterityProficient, ConstitutionValue, ConstitutionProficient, IntelligenceValue, " +
                "IntelligenceProficient, WisdomValue, WisdomProficient, CharismaValue, CharismaProficient, MaxHp, CurrentHp, TemporaryHp, MaxTemporaryHp, " +
                "HpDice, ArmorClass, SpellDifficulty, Movement, Athletics, Acrobatics, SleightofHand, Stealth, Arcana, History, Investigation, Nature, " +
                "Religion, AnimalHandling, Insight, Medicine, Perception, Survival, Deception, Intimidation, Performance, Persuasion, " +
                "Inspiration, Size, Vision, DomainsAndLanguages, Appearance, CharacterTraits, Ideals, Affection, Weaknesses) " +
                "VALUES (@CampaignId, @Name, @PlayerName, @Class, @Race, @Level, @Experience, " +
                "@StrengthValue, @StrengthProficient, @DexterityValue, @DexterityProficient, @ConstitutionValue, @ConstitutionProficient, " +
                "@IntelligenceValue, @IntelligenceProficient, @WisdomValue, @WisdomProficient, @CharismaValue, @CharismaProficient, " +
                "@MaxHp, @CurrentHp, @TemporaryHp, @MaxTemporaryHp, @HpDice, @ArmorClass, @SpellDifficulty, @Movement, " +
                "@Athletics, @Acrobatics, @SleightofHand, @Stealth, @Arcana, @History, @Investigation, @Nature, @Religion, " +
                "@AnimalHandling, @Insight, @Medicine, @Perception, @Survival, @Deception, @Intimidation, @Performance, @Persuasion, " +
                "@Inspiration, @Size, @Vision, @DomainsAndLanguages, @Appearance, @CharacterTraits, @Ideals, @Affection, @Weaknesses)", connection);
            command.Parameters.AddWithValue("@CampaignId", character.CampaignId);
            command.Parameters.AddWithValue("@Name", character.Name);
            command.Parameters.AddWithValue("@PlayerName", character.PlayerName);
            command.Parameters.AddWithValue("@Class", character.Class);
            command.Parameters.AddWithValue("@Race", character.Race);
            command.Parameters.AddWithValue("@Level", character.Level);
            command.Parameters.AddWithValue("@Experience", character.Experience);
            command.Parameters.AddWithValue("@StrengthValue", character.Characteristics.StrengthValue);
            command.Parameters.AddWithValue("@StrengthProficient", character.Characteristics.StrengthProficient);
            command.Parameters.AddWithValue("@DexterityValue", character.Characteristics.DexterityValue);
            command.Parameters.AddWithValue("@DexterityProficient", character.Characteristics.DexterityProficient);
            command.Parameters.AddWithValue("@ConstitutionValue", character.Characteristics.ConstitutionValue);
            command.Parameters.AddWithValue("@ConstitutionProficient", character.Characteristics.ConstitutionProficient);
            command.Parameters.AddWithValue("@IntelligenceValue", character.Characteristics.IntelligenceValue);
            command.Parameters.AddWithValue("@IntelligenceProficient", character.Characteristics.IntelligenceProficient);
            command.Parameters.AddWithValue("@WisdomValue", character.Characteristics.WisdomValue);
            command.Parameters.AddWithValue("@WisdomProficient", character.Characteristics.WisdomProficient);
            command.Parameters.AddWithValue("@CharismaValue", character.Characteristics.CharismaValue);
            command.Parameters.AddWithValue("@CharismaProficient", character.Characteristics.CharismaProficient);
            command.Parameters.AddWithValue("@MaxHp", character.MaxHp);
            command.Parameters.AddWithValue("@CurrentHp", character.CurrentHp);
            command.Parameters.AddWithValue("@TemporaryHp", character.TemporaryHp);
            command.Parameters.AddWithValue("@MaxTemporaryHp", character.MaxTemporaryHp);
            command.Parameters.AddWithValue("@HpDice", character.HpDice);
            command.Parameters.AddWithValue("@ArmorClass", character.ArmorClass);
            command.Parameters.AddWithValue("@SpellDifficulty", character.SpellDifficulty);
            command.Parameters.AddWithValue("@Movement", character.Movement);
            command.Parameters.AddWithValue("@Athletics", character.Skills.Athletics);
            command.Parameters.AddWithValue("@Acrobatics", character.Skills.Acrobatics);
            command.Parameters.AddWithValue("@SleightofHand", character.Skills.SleightofHand);
            command.Parameters.AddWithValue("@Stealth", character.Skills.Stealth);
            command.Parameters.AddWithValue("@Arcana", character.Skills.Arcana);
            command.Parameters.AddWithValue("@History", character.Skills.History);
            command.Parameters.AddWithValue("@Investigation", character.Skills.Investigation);
            command.Parameters.AddWithValue("@Nature", character.Skills.Nature);
            command.Parameters.AddWithValue("@Religion", character.Skills.Religion);
            command.Parameters.AddWithValue("@AnimalHandling", character.Skills.AnimalHandling);
            command.Parameters.AddWithValue("@Insight", character.Skills.Insight);
            command.Parameters.AddWithValue("@Medicine", character.Skills.Medicine);
            command.Parameters.AddWithValue("@Perception", character.Skills.Perception);
            command.Parameters.AddWithValue("@Survival", character.Skills.Survival);
            command.Parameters.AddWithValue("@Deception", character.Skills.Deception);
            command.Parameters.AddWithValue("@Intimidation", character.Skills.Intimidation);
            command.Parameters.AddWithValue("@Performance", character.Skills.Performance);
            command.Parameters.AddWithValue("@Persuasion", character.Skills.Persuasion);
            command.Parameters.AddWithValue("@Inspiration", character.Inspiration);
            command.Parameters.AddWithValue("@Size", character.Size);
            command.Parameters.AddWithValue("@Vision", character.Vision);
            command.Parameters.AddWithValue("@DomainsAndLanguages", character.DomainsAndLanguages);
            command.Parameters.AddWithValue("@Appearance", character.Appearance);
            command.Parameters.AddWithValue("@CharacterTraits", character.CharacterTraits);
            command.Parameters.AddWithValue("@Ideals", character.Ideals);
            command.Parameters.AddWithValue("@Affection", character.Affection);
            command.Parameters.AddWithValue("@Weaknesses", character.Weaknesses);

            command.ExecuteNonQuery();
        }

        public void UpdateCharacter(dbCharacterData character)
        {
            using var connection = new NpgsqlConnection(connectionString);
            Console.WriteLine("Update");
            connection.Open();

            using var command = new NpgsqlCommand("UPDATE EvermoreCharacters SET Name = @Name, " +
                "PlayerName = @PlayerName, Class = @Class, Race = @Race, Level = @Level, Experience = @Experience, " +
                "StrengthValue = @StrengthValue, StrengthProficient = @StrengthProficient, DexterityValue = @DexterityValue, " +
                "DexterityProficient = @DexterityProficient, ConstitutionValue = @ConstitutionValue, ConstitutionProficient = @ConstitutionProficient, " +
                "IntelligenceValue = @IntelligenceValue, IntelligenceProficient = @IntelligenceProficient, WisdomValue = @WisdomValue, " +
                "WisdomProficient = @WisdomProficient, CharismaValue = @CharismaValue, CharismaProficient = @CharismaProficient, " +
                "MaxHp = @MaxHp, CurrentHp = @CurrentHp, TemporaryHp = @TemporaryHp, MaxTemporaryHp = @MaxTemporaryHp, " +
                "HpDice = @HpDice, ArmorClass = @ArmorClass, SpellDifficulty = @SpellDifficulty, Movement = @Movement, " +
                "Athletics = @Athletics, Acrobatics = @Acrobatics, SleightofHand = @SleightofHand, Stealth = @Stealth, " +
                "Arcana = @Arcana, History = @History, Investigation = @Investigation, Nature = @Nature, " +
                "Religion = @Religion, AnimalHandling = @AnimalHandling, Insight = @Insight, Medicine = @Medicine, " +
                "Perception = @Perception, Survival = @Survival, Deception = @Deception, Intimidation = @Intimidation, " +
                "Performance = @Performance, Persuasion = @Persuasion, Inspiration = @Inspiration, " +
                "Size = @Size, Vision = @Vision, DomainsAndLanguages = @DomainsAndLanguages, Appearance = @Appearance, " +
                "CharacterTraits = @CharacterTraits, Ideals = @Ideals, Affection = @Affection, Weaknesses = @Weaknesses " +
                "WHERE CharacterId = @CharacterId", connection);
            command.Parameters.AddWithValue("@CharacterId", character.CharacterId);
            command.Parameters.AddWithValue("@Name", character.Name);
            command.Parameters.AddWithValue("@PlayerName", character.PlayerName);
            command.Parameters.AddWithValue("@Class", character.Class);
            command.Parameters.AddWithValue("@Race", character.Race);
            command.Parameters.AddWithValue("@Level", character.Level);
            command.Parameters.AddWithValue("@Experience", character.Experience);
            command.Parameters.AddWithValue("@StrengthValue", character.Characteristics.StrengthValue);
            command.Parameters.AddWithValue("@StrengthProficient", character.Characteristics.StrengthProficient);
            command.Parameters.AddWithValue("@DexterityValue", character.Characteristics.DexterityValue);
            command.Parameters.AddWithValue("@DexterityProficient", character.Characteristics.DexterityProficient);
            command.Parameters.AddWithValue("@ConstitutionValue", character.Characteristics.ConstitutionValue);
            command.Parameters.AddWithValue("@ConstitutionProficient", character.Characteristics.ConstitutionProficient);
            command.Parameters.AddWithValue("@IntelligenceValue", character.Characteristics.IntelligenceValue);
            command.Parameters.AddWithValue("@IntelligenceProficient", character.Characteristics.IntelligenceProficient);
            command.Parameters.AddWithValue("@WisdomValue", character.Characteristics.WisdomValue);
            command.Parameters.AddWithValue("@WisdomProficient", character.Characteristics.WisdomProficient);
            command.Parameters.AddWithValue("@CharismaValue", character.Characteristics.CharismaValue);
            command.Parameters.AddWithValue("@CharismaProficient", character.Characteristics.CharismaProficient);
            command.Parameters.AddWithValue("@MaxHp", character.MaxHp);
            command.Parameters.AddWithValue("@CurrentHp", character.CurrentHp);
            command.Parameters.AddWithValue("@TemporaryHp", character.TemporaryHp);
            command.Parameters.AddWithValue("@MaxTemporaryHp", character.MaxTemporaryHp);
            command.Parameters.AddWithValue("@HpDice", character.HpDice);
            command.Parameters.AddWithValue("@ArmorClass", character.ArmorClass);
            command.Parameters.AddWithValue("@SpellDifficulty", character.SpellDifficulty);
            command.Parameters.AddWithValue("@Movement", character.Movement);
            command.Parameters.AddWithValue("@Athletics", character.Skills.Athletics);
            command.Parameters.AddWithValue("@Acrobatics", character.Skills.Acrobatics);
            command.Parameters.AddWithValue("@SleightofHand", character.Skills.SleightofHand);
            command.Parameters.AddWithValue("@Stealth", character.Skills.Stealth);
            command.Parameters.AddWithValue("@Arcana", character.Skills.Arcana);
            command.Parameters.AddWithValue("@History", character.Skills.History);
            command.Parameters.AddWithValue("@Investigation", character.Skills.Investigation);
            command.Parameters.AddWithValue("@Nature", character.Skills.Nature);
            command.Parameters.AddWithValue("@Religion", character.Skills.Religion);
            command.Parameters.AddWithValue("@AnimalHandling", character.Skills.AnimalHandling);
            command.Parameters.AddWithValue("@Insight", character.Skills.Insight);
            command.Parameters.AddWithValue("@Medicine", character.Skills.Medicine);
            command.Parameters.AddWithValue("@Perception", character.Skills.Perception);
            command.Parameters.AddWithValue("@Survival", character.Skills.Survival);
            command.Parameters.AddWithValue("@Deception", character.Skills.Deception);
            command.Parameters.AddWithValue("@Intimidation", character.Skills.Intimidation);
            command.Parameters.AddWithValue("@Performance", character.Skills.Performance);
            command.Parameters.AddWithValue("@Persuasion", character.Skills.Persuasion);
            command.Parameters.AddWithValue("@Inspiration", character.Inspiration);
            command.Parameters.AddWithValue("@Size", character.Size);
            command.Parameters.AddWithValue("@Vision", character.Vision);
            command.Parameters.AddWithValue("@DomainsAndLanguages", character.DomainsAndLanguages);
            command.Parameters.AddWithValue("@Appearance", character.Appearance);
            command.Parameters.AddWithValue("@CharacterTraits", character.CharacterTraits);
            command.Parameters.AddWithValue("@Ideals", character.Ideals);
            command.Parameters.AddWithValue("@Affection", character.Affection);
            command.Parameters.AddWithValue("@Weaknesses", character.Weaknesses);

            command.ExecuteNonQuery();
        }

        public void DeleteCharacter(int characterId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("DELETE FROM EvermoreCharacters WHERE CharacterId = @CharacterId", connection);
            command.Parameters.AddWithValue("@CharacterId", characterId);

            command.ExecuteNonQuery();
        }
        #endregion

        #region Locations, organizations, quests, monsters

        // Location
        public List<Location> GetAllLocations(int CampaignId)
        {
            List<Location> locations = new List<Location>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using var command = new NpgsqlCommand("SELECT * FROM Locations WHERE CampaignId = @CampaignId", connection);
                command.Parameters.AddWithValue("@CampaignId", CampaignId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Location location = new Location
                    {
                        Id = reader.GetInt32(0),
                        CampaignId = reader.GetInt32(1),
                        Name = reader.GetString(2)
                    };

                    locations.Add(location);
                }
            }

            return locations;
        }

        public void AddLocation(Location location)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO Locations (CampaignId, Name) " +
                "VALUES (@CampaignId, @Name)", connection);
            command.Parameters.AddWithValue("@CampaignId", location.CampaignId);
            command.Parameters.AddWithValue("@Name", location.Name);

            command.ExecuteNonQuery();
        }

        public void UpdateLocation(Location location)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("UPDATE Locations SET Name = @Name " +
                "WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", location.Id);
            command.Parameters.AddWithValue("@Name", location.Name);

            command.ExecuteNonQuery();
        }

        public void DeleteLocation(int locationId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("DELETE FROM Locations WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", locationId);

            command.ExecuteNonQuery();
        }

        // Organization

        public List<Organization> GetAllOrganizations(int id)
        {
            List<Organization> organizations = new List<Organization>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using var command = new NpgsqlCommand("SELECT * FROM Organizations WHERE LocationId = @LocationId", connection);
                command.Parameters.AddWithValue("@LocationId", id);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Organization organization = new Organization
                    {
                        Id = reader.GetInt32("Id"),
                        LocationId = reader.GetInt32("LocationId"),
                        Title = reader.GetString("Title"),
                        Description = reader.GetString("Description")
                    };

                    organizations.Add(organization);
                }
            }

            return organizations;
        }

        public void AddOrganization(Organization organization)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO Organizations (LocationId, Title, Description) " +
                "VALUES (@LocationId, @Title, @Description)", connection);
            command.Parameters.AddWithValue("@LocationId", organization.LocationId);
            command.Parameters.AddWithValue("@Title", organization.Title);
            command.Parameters.AddWithValue("@Description", organization.Description);

            command.ExecuteNonQuery();
        }

        public void UpdateOrganization(Organization organization)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("UPDATE Organizations SET LocationId = @LocationId, " +
                "Title = @Title, Description = @Description WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", organization.Id);
            command.Parameters.AddWithValue("@LocationId", organization.LocationId);
            command.Parameters.AddWithValue("@Title", organization.Title);
            command.Parameters.AddWithValue("@Description", organization.Description);

            command.ExecuteNonQuery();
        }

        public void DeleteOrganization(int organizationId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("DELETE FROM Organizations WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", organizationId);

            command.ExecuteNonQuery();
        }

        // Quest

        public List<Quest> GetAllQuests(int id)
        {
            List<Quest> quests = new List<Quest>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using var command = new NpgsqlCommand("SELECT * FROM Quests WHERE LocationId = @LocationId", connection);
                command.Parameters.AddWithValue("@LocationId", id);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Quest quest = new Quest
                    {
                        Id = reader.GetInt32("Id"),
                        LocationId = reader.GetInt32("LocationId"),
                        Title = reader.GetString("Title"),
                        Description = reader.GetString("Description")
                    };

                    quests.Add(quest);
                }
            }

            return quests;
        }

        public void AddQuest(Quest quest)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO Quests (LocationId, Title, Description) " +
                "VALUES (@LocationId, @Title, @Description)", connection);
            command.Parameters.AddWithValue("@LocationId", quest.LocationId);
            command.Parameters.AddWithValue("@Title", quest.Title);
            command.Parameters.AddWithValue("@Description", quest.Description);

            command.ExecuteNonQuery();
        }

        public void UpdateQuest(Quest quest)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("UPDATE Quests SET LocationId = @LocationId, " +
                "Title = @Title, Description = @Description WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", quest.Id);
            command.Parameters.AddWithValue("@LocationId", quest.LocationId);
            command.Parameters.AddWithValue("@Title", quest.Title);
            command.Parameters.AddWithValue("@Description", quest.Description);

            command.ExecuteNonQuery();
        }

        public void DeleteQuest(int questId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("DELETE FROM Quests WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", questId);

            command.ExecuteNonQuery();
        }

        // Monster
        public List<Monster> GetAllMonsters(int id)
        {
            List<Monster> monsters = new List<Monster>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using var command = new NpgsqlCommand("SELECT * FROM Monsters WHERE LocationId = @LocationId", connection);
                command.Parameters.AddWithValue("@LocationId", id);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Monster monster = new Monster
                    {
                        Id = reader.GetInt32("Id"),
                        LocationId = reader.GetInt32("LocationId"),
                        Name = reader.GetString("Name"),
                        Size = reader.GetString("Size"),
                        Type = reader.GetString("Type"),
                        Outlook = reader.GetString("Outlook"),
                        Description = reader.GetString("Description")
                    };

                    monsters.Add(monster);
                }
            }

            return monsters;
        }

        public void AddMonster(Monster monster)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO Monsters (LocationId, Name, Size, Type, Outlook, Description) " +
                "VALUES (@LocationId, @Name, @Size, @Type, @Outlook, @Description)", connection);
            command.Parameters.AddWithValue("@LocationId", monster.LocationId);
            command.Parameters.AddWithValue("@Name", monster.Name);
            command.Parameters.AddWithValue("@Size", monster.Size);
            command.Parameters.AddWithValue("@Type", monster.Type);
            command.Parameters.AddWithValue("@Outlook", monster.Outlook);
            command.Parameters.AddWithValue("@Description", monster.Description);

            command.ExecuteNonQuery();
        }

        public void UpdateMonster(Monster monster)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("UPDATE Monsters SET LocationId = @LocationId, " +
                "Name = @Name, Size = @Size, Type = @Type, Outlook = @Outlook, Description = @Description " +
                "WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", monster.Id);
            command.Parameters.AddWithValue("@LocationId", monster.LocationId);
            command.Parameters.AddWithValue("@Name", monster.Name);
            command.Parameters.AddWithValue("@Size", monster.Size);
            command.Parameters.AddWithValue("@Type", monster.Type);
            command.Parameters.AddWithValue("@Outlook", monster.Outlook);
            command.Parameters.AddWithValue("@Description", monster.Description);

            command.ExecuteNonQuery();
        }

        public void DeleteMonster(int monsterId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("DELETE FROM Monsters WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", monsterId);

            command.ExecuteNonQuery();
        }

        #endregion

        #region Notes
        public List<Note> GetNotes(int CampaignId)
        {
            List<Note> notes = new List<Note>();

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using (var command = new NpgsqlCommand("SELECT * FROM Notes WHERE CampaignId = @CampaignId", connection))
            {
                command.Parameters.AddWithValue("@CampaignId", CampaignId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var note = new Note
                    {
                        Id = reader.GetInt32("Id"),
                        CampaignId = reader.GetInt32("CampaignId"),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString("Title"),
                        NoteText = reader.IsDBNull(reader.GetOrdinal("NoteText")) ? null : reader.GetString("NoteText"),
                    };

                    notes.Add(note);
                }
            }

            return notes;
        }
        public void AddNewNote(Note note)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("INSERT INTO Notes (CampaignId, Title, NoteText) " +
                "VALUES (@CampaignId, @Title, @NoteText)", connection);
            command.Parameters.AddWithValue("@CampaignId", note.CampaignId);
            command.Parameters.AddWithValue("@Title", note.Title);
            command.Parameters.AddWithValue("@NoteText", note.NoteText);

            command.ExecuteNonQuery();
        }

        public void UpdateNote(Note note)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("UPDATE Notes SET Title = @Title, NoteText = @NoteText " +
                "WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", note.Id);
            command.Parameters.AddWithValue("@Title", note.Title);
            command.Parameters.AddWithValue("@NoteText", note.NoteText);

            command.ExecuteNonQuery();
        }

        public void DeleteNote(Note note)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("DELETE FROM Notes WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", note.Id);

            command.ExecuteNonQuery();
        }
        #endregion
    }
}