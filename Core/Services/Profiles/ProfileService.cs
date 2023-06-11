using DB;
using DB.Models.Items;
using DB.Models.Items.Enums;
using DB.Models.Profiles;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Profiles
{
    public interface IProfileService
    {
        /// <summary>
        /// Gets discord profile if exists in database, otherwise creates new one and returns it
        /// </summary>
        /// <param name="discordID"></param>
        /// <param name="guildID"></param>
        /// <returns>Found or created profile</returns>
        Task<Profile> GetOrCreateProfileAsync(ulong discordID, ulong guildID);
        Task<Profile> GetProfileWithEquipmentStats(ulong discordID, ulong guildID);

        /// <summary>
        /// Uses item if player has it in his inventory
        /// </summary>
        /// <param name="discordID"></param>
        /// <param name="guildID"></param>
        /// <param name="itemName"></param>
        /// <returns>true if item was found, false if could'nt find item in inventory</returns>
        Task<bool> UseItemAsync(ulong discordID, ulong guildID, IItem item);
        Task ClearPotions(ulong discordID, ulong guildID);

        /// <summary>
        /// levels up profile skill by amount
        /// </summary>
        /// <param name="discordID"></param>
        /// <param name="guildID"></param>
        /// <param name="skill"></param>
        /// <param name="amount"></param>
        /// <returns>true if everything went ok, false if skill type was none or undefined</returns>
        Task<bool> LevelUpSkill(ulong discordID, ulong guildID, SkillType skill, int amount, ulong goldPrice);
        Task AddGold(ulong discordID, ulong guildID, int amount);
        Task UpdateProfile(Profile profile);
        Task<EquipmentStats> GetEquipmentStats(ulong discordID, ulong guildID);
        Task<Dictionary<Modifiers, double>> GetEquipmentModifiers(ulong discordID, ulong guildID);
    }

        /// <summary>
        /// Wrapper class for storing info about equipment statistics
        /// Basicly it shares same values as item
        /// </summary>
        public class EquipmentStats
        {
            public int Strength { get; internal set; }
            public int Agility { get; internal set; }
            public int Intelligence { get; internal set; }
            public int Endurance { get; internal set; }
            public int Luck { get; internal set; }
            public int Armor { get; internal set; }
        }

    public class ProfileService: IProfileService
    {
        private readonly DbContextOptions<Context> _options;
        public ProfileService(DbContextOptions<Context> options)
        {
            _options = options;
        }

        public async Task AddGold(ulong discordID, ulong guildID, int amount)
        {
            using var _context = new Context(_options);

            Profile profile = await GetOrCreateProfileAsync(discordID, guildID);

            profile.Gold += amount;

            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        public async Task<Profile> GetOrCreateProfileAsync(ulong discordID, ulong guildID)
        {
            using var _context = new Context(_options);

            Profile? profile = await _context.Profiles.Where(x => x.GuildID == guildID)
                    .Include(x => x.Items)
                    .Include(x=>x.Equipment)
                    .Include(x=> x.ShopItems)
                    .FirstOrDefaultAsync(x => x.DiscordID == discordID)
                    .ConfigureAwait(false);

            if (profile != null)
                return profile;

            profile = new Profile
            {
                DiscordID = discordID,
                GuildID = guildID,
                lastFightTime = DateTime.Now,
                lastQuestTime = DateTime.Now,
                nextFightTime = DateTime.Now,
                nextQuestTime = DateTime.Now,
                lastFreeRerollTime = DateTime.Now
            };

            //Add equipment slots
            for (int x = 1; x <= 9; x++)
                profile.Equipment.Add(new EquipmentItem(profile.ID, (ItemType)x));

            //Add potion/consumable slots
            for (int x = 0; x < 3; x++)
                profile.Equipment.Add(new EquipmentItem(profile.ID, ItemType.Potion));

            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);



            return profile;
        }

        public async Task<bool> LevelUpSkill(ulong discordID, ulong guildID, SkillType skill, int amount, ulong goldPrice)
        {
            using var _context = new Context(_options);


            Profile profile = await GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);

            switch (skill)
            {
                case SkillType.None:
                    return false;
                case SkillType.Strength:
                    profile.Strength += amount;
                    break;
                case SkillType.Agility:
                    profile.Agility += amount;
                    break;
                case SkillType.Intelligence:
                    profile.Intelligence += amount;
                    break;
                case SkillType.Endurance:
                    profile.Endurance += amount;
                    break;
                case SkillType.Luck:
                    profile.Luck += amount;
                    break;
                default:
                    return false;
            }

            profile.Gold -= goldPrice;

            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }

        public async Task UpdateProfile(Profile profile)
        {
            using var _context = new Context(_options);

            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> UseItemAsync(ulong discordID, ulong guildID, IItem item)
        {
            using var _context = new Context(_options);

            Profile profile = await GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);

            var Item = profile.Items.FirstOrDefault(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));

            if (Item != null)
            {
                profile.Items.Remove(Item);
                _context.ProfileItems.Remove(Item);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }

            //item has not been found in inventory
            return false;
        }


        public async Task<EquipmentStats> GetEquipmentStats(ulong discordID, ulong guildID)
        {
            using var _context = new Context(_options);
            Profile profile = await GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);
            EquipmentStats output = new();

            foreach(var item in profile.Equipment)
            {
                output.Strength+=item.Strength;
                output.Agility+=item.Agility;
                output.Intelligence+=item.Intelligence;
                output.Endurance+=item.Endurance;
                output.Luck+=item.Luck;
                output.Armor+=item.Armor;
            }

            return output;
        }

        public async Task<Dictionary<Modifiers, double>> GetEquipmentModifiers(ulong discordID, ulong guildID)
        {
            using var _context = new Context(_options);
            Profile profile = await GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);

            Dictionary<Modifiers, double> output = new();

            foreach(var item in profile.Equipment)
            {
                var modifiers = Modifier.GetModifiersFromString(item.Modifiers);
                foreach(var modifier in modifiers)
                    output[modifier.modifier] += modifier.BonusPercent/100;
            }

            return output;
        }

        public async Task ClearPotions(ulong discordID, ulong guildID)
        {
            using var _context = new Context(_options);
            Profile profile = await GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);

            for(int x = 9; x<12; x++)
                profile.Equipment[x].ChangeItemProperties(new EquipmentItem(profile.ID, ItemType.Potion ));

            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Profile> GetProfileWithEquipmentStats(ulong discordID, ulong guildID)
        {
            using var _context = new Context(_options);
            Profile profile = await GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);
            EquipmentStats eqStats = await GetEquipmentStats(discordID, guildID).ConfigureAwait(false);

            profile.Strength += eqStats.Strength;
            profile.Agility += eqStats.Agility;
            profile.Intelligence += eqStats.Intelligence;
            profile.Endurance += eqStats.Endurance;
            profile.Luck += eqStats.Luck;
            profile.Armor += eqStats.Armor;

            return profile;
        }
    }
}
