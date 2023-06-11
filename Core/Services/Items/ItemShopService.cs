using Core.Services.Profiles;
using DB;
using DB.Models.Items;
using DB.Models.Items.Enums;
using DB.Models.Profiles;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Items
{
    public interface IItemShopService
    {
        /// <summary>
        /// Returns list of random items scaled to specific level
        /// </summary>
        /// <param name="amount">Amount of items to get</param>
        /// <returns></returns>
        public Task<List<IItem>> GetShopItems(int amount, int level);

        /// <summary>
        /// Gets or Creates player shop if one does not exist
        /// Rerolls shop if needed
        /// </summary>
        /// <returns>Player's itemshop items</returns>
        public Task<List<IItem>> GetOrCreatePlayerShop(ulong discordID, ulong guildID);
        /// <summary>
        /// Rerolls item shop items to the new ones
        /// </summary>
        /// <param name="freeReroll">Determines whether reroll is daily free reroll and updates
        /// lastFreeRerollTime property of profile to actual time
        /// </param>
        /// <returns>false when user did not have enough gold to reroll</returns>
        public Task<bool> RerollItemShop(ulong discordID, ulong guildID, int goldCost, bool freeReroll=false);

        /// <summary>
        /// Changes item at given index to the new one
        /// Remember that indexes at tables are counted from 0!
        /// </summary>
        /// <returns>return false if item could not be changed properly</returns>
        public Task<bool> ChangeShopItem(ulong discordID, ulong guildID, int index, IItem item);
    }
    public class ItemShopService :IItemShopService
    {
        private readonly DbContextOptions<Context> _options;
        private readonly IProfileService _profileService;
        private readonly IItemService _itemService;

        public static readonly int ITEMSHOP_ITEM_AMOUNT = 12;

        public ItemShopService(DbContextOptions<Context> options, IProfileService profileService, IItemService itemService)
        {
            _options = options;
            _profileService = profileService;
            _itemService = itemService;
        }

        public async Task<List<IItem>> GetShopItems(int amount, int level)
        {
            List<IItem> output = new List<IItem>();
            for (int x = 0; x < amount; x++)
            {
                IItem randomItem = await _itemService.GetRandomItem().ConfigureAwait(false);

                //from 0 to 3 modifiers
                int modifiersAmount = Math.BotMath.RandomNumberGenerator.Next(0, 4);

                //potions and miscellaneous items should not get modifiers
                if(randomItem.Type!=ItemType.Potion && randomItem.Type!=ItemType.Miscellaneous)
                    randomItem.Modifiers = _itemService.GetItemRandomModifiers((uint)modifiersAmount);

                output.Add(_itemService.ScaleItem(randomItem, level));
            }

            return output;
        }
        
        public async Task<List<IItem>> GetOrCreatePlayerShop(ulong discordID, ulong guildID)
        {
            var _context = new Context(_options);

            Profile profile = await _profileService.GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);

            //if player has no itemshop yet, create one
            if (profile.ShopItems.Count == 0)
            {
                var shopItems = await GetShopItems(ITEMSHOP_ITEM_AMOUNT, profile.Level);
                foreach (var item in shopItems)
                    profile.ShopItems.Add(new ShopItem(profile.ID, item));

                _context.Update(profile);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            //here we check whether we should force daily reroll
            //we cast total days to int to get rid of fractions, we need to know if the 00:00:00 passed and day changed
            //then we force free reroll
            if (((int)TimeSpan.FromTicks(DateTime.Now.Ticks).TotalDays) > ((int)TimeSpan.FromTicks(profile.lastFreeRerollTime.Ticks).TotalDays))
                await RerollItemShop(profile.DiscordID, profile.GuildID, 0, true).ConfigureAwait(false);


            List<IItem> output = new List<IItem>();

            //cast all shop items to IItem, we cannot cast whole list at once
            foreach (var item in profile.ShopItems)
                output.Add(item);

            return output;
        }

        public async Task<bool> RerollItemShop(ulong discordID, ulong guildID, int goldCost, bool freeReroll=false)
        {
            var _context = new Context(_options);

            Profile profile = await _profileService.GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);

            if (profile.Gold < goldCost)
                return false;

            var newItems =  await GetShopItems(ITEMSHOP_ITEM_AMOUNT, profile.Level).ConfigureAwait(false);

            for(int x=0;x<profile.ShopItems.Count;x++)
                ((IItem)(profile.ShopItems[x])).ChangeItemProperties(newItems[x]);

            profile.Gold -= goldCost;
            if (freeReroll)
                profile.lastFreeRerollTime = DateTime.Now;

            _context.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }

        public async Task<bool> ChangeShopItem(ulong discordID, ulong guildID, int index, IItem item)
        {
            var _context = new Context(_options);

            Profile profile = await _profileService.GetOrCreateProfileAsync(discordID, guildID).ConfigureAwait(false);

            try
            {
                ((IItem)(profile.ShopItems[index])).ChangeItemProperties(item);
            }
    #pragma warning disable
            catch(Exception e)
            {
                return false;
            }
    #pragma warning restore
            _context.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }
    }
}
