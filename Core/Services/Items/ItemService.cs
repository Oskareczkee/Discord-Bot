using Core.Math;
using Core.Services.Profiles;
using DB;
using DB.Models.Items;
using DB.Models.Items.Enums;
using DB.Models.Profiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Items
{
    public enum ItemPurchaseState {ItemNotFound, NotEnoughGold, EverythingWentGood }
    public interface IItemService
    {
        Task CreateNewItemAsync(IItem item, bool isEpicItem=false);
        Task UpdateItemAsync(IItem item);
        Task<IItem> GetItemByName(string name);

        Task<ItemPurchaseState> PurchaseItemAsync(ulong discordId, ulong guildID, IItem item);

        Task AddItemAsync(ulong discordID, ulong guildID, IItem item);

        Task<List<IItem>> GetItemsListAsync();
        Task<IItem> GetRandomItem();
        Task<bool> EquipItemAsync(ulong discordID, ulong guildID, IItem item);
        Task<bool> UseConsumableAsync(ulong discordID, ulong guildID, IItem? consumable, int potionSlot);

        /// <summary>
        /// Scales item to given level
        /// </summary>
        /// <param name="item"></param>
        /// <param name="level"></param>
        /// <param name="useRandomFactors">Determines whether item stats should be scaled by some random factor</param>
        /// <returns></returns>
        IItem ScaleItem(IItem item, int level, bool useRandomFactors=true);

        //minValue and maxValue cam be overidden for epic items to make them have only positive modifiers
        string GetItemRandomModifiers(uint amount, int minValue=-10, int maxValue=10);
    }


    public class ItemService: IItemService
    {

        private readonly DbContextOptions<Context> _options;
        private readonly IProfileService _profileService;

        public ItemService(DbContextOptions<Context> options, IProfileService profileService)
        {
            _options = options;
            _profileService = profileService;
        }
         
        /// <summary>
        /// Find an item by name
        /// </summary>
        /// <returns>Default if item was not found</returns>
        public async Task<IItem> GetItemByName(string name)
        {
            using var _context = new Context(_options);

            //check for item in ordinary items database
            IItem item = await _context.Items.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()).ConfigureAwait(false) ?? null!;
            return item;

        }

        public async Task CreateNewItemAsync(IItem item, bool isEpicItem=false)
        {
            using var _context = new Context(_options);

           if(!isEpicItem)
              await _context.Items.AddAsync((ItemBase)item).ConfigureAwait(false);
           //else
                //await _context.EpicItems.AddAsync(new EpicItem(item)).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateItemAsync(IItem item)
        {
            using var _context = new Context(_options);

            _context.Items.Update((ItemBase)item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<ItemPurchaseState> PurchaseItemAsync(ulong discordId, ulong guildID, IItem item)
        {
            using var context = new Context(_options);

            if (item == null)
                return ItemPurchaseState.ItemNotFound;

            Profile profile = await _profileService.GetOrCreateProfileAsync(discordId, guildID).ConfigureAwait(false);

            if (profile.Gold < item.Price)
                return ItemPurchaseState.NotEnoughGold;

            profile.Gold -= item.Price;

            profile.Items.Add(new ProfileItem(profile.ID, item));

            context.Profiles.Update(profile);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return ItemPurchaseState.EverythingWentGood;

        }


        public async Task<List<IItem>> GetItemsListAsync()
        {
            using var context = new Context(_options);
            List<IItem> Items = await context.Items.Cast<IItem>().ToListAsync().ConfigureAwait(false);

            return Items;
        }

        public async Task AddItemAsync(ulong discordID, ulong guildID, IItem item)
        {
            //this function basically works like purchase item, but does not require gold, takes actual item as an argument

            using var context = new Context(_options);

            Profile profile = await _profileService.GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);

            profile.Items.Add(new ProfileItem(profile.ID, item));

            context.Profiles.Update(profile);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IItem> GetRandomItem()
        {
            using var _context = new Context(_options);
            int randomIndex = BotMath.RandomNumberGenerator.Next(0, _context.Items.Count());

            return await _context.Items.Skip(randomIndex).Take(1).FirstAsync();
        }

        public async Task<bool> EquipItemAsync(ulong discordID, ulong guildID, IItem item)
        {
            using var _context = new Context(_options);

            Profile profile = await _profileService.GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);

            int index = (int)(item.Type) - 1;

            if (profile.Equipment[index].Name.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                if (item.Name.Equals("None"))
                    return false;

                profile.Equipment[index].ChangeItemProperties(item);
                _context.Profiles.Update(profile);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                await _profileService.UseItemAsync(discordID,guildID,item);
                return true;
            }
            else
            {
                await AddItemAsync(discordID, guildID, profile.Equipment[index]);
                profile.Equipment[index].ChangeItemProperties(item);
                _context.Profiles.Update(profile);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                await _profileService.UseItemAsync(discordID, guildID,item);
                return true;
            }
        }

        public async Task<bool> UseConsumableAsync(ulong discordID, ulong guildID, IItem? consumable, int potionSlot)
        {
            //max 3 potion slots
            if (consumable!.Type != ItemType.Potion || potionSlot>3 || potionSlot<1)
                return false;

            //match the potion indexes in equipment
            int index =potionSlot +8;

            using var _context = new Context(_options);
            Profile profile = await _profileService.GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);
            //indexes 9-11 are reserved for consumables

            //player wants to add none item to empty slot
            if (consumable.Name.Equals("None", StringComparison.OrdinalIgnoreCase) && profile.Equipment[index].Name.Equals("None", StringComparison.OrdinalIgnoreCase))
                return false;

            //potions are irreplacable, we do not give it back to equipment
            profile.Equipment[index].ChangeItemProperties(consumable);
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            await _profileService.UseItemAsync(discordID, guildID, consumable);
            return true;
        }

        public IItem ScaleItem(IItem item, int level, bool useRandomFactors=true)
        {
            return BotMath.ScaleItem(item, level, useRandomFactors);
        }

        public string GetItemRandomModifiers(uint amount, int minValue=-10, int maxValue=10)
        {
            string output = string.Empty;

            var randomModifiers = Enumerable.Range(1, Enum.GetValues(typeof(Modifiers)).Length-1).OrderBy(x => BotMath.RandomNumberGenerator.Next()).Take((int)amount).ToList();

            for (int x = 0; x < amount; x++)
            {
                int modifier = randomModifiers[x];
                int value = BotMath.RandomNumberGenerator.Next(minValue, maxValue+1);

                //just to prevent from 0% modifiers
                if (value == 0)
                    value = 1;

                output += modifier.ToString() + " " + value.ToString() + " ";
            }
            //remove space at the end
            output = output.Trim();

            return output;
        }
    }
}
