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
    public interface IItemService
    {
        Task CreateNewItemAsync(IItem item,ulong guildID, bool isEpicItem=false);
        Task UpdateItemAsync(IItem item, ulong guildID);
        Task<IItem> GetItemByNameAsync(string name, ulong guildID);

        /// <summary>
        /// Gets list of items on server
        /// </summary>
        Task<List<IItem>> GetItemsListAsync(ulong guildID);
        /// <summary>
        /// Gets random item from given server
        /// </summary>
        Task<IItem> GetRandomItemAsync(ulong guildID);

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

        public ItemService(DbContextOptions<Context> options, IProfileService profileService) => _options = options;
    
        /// <summary>
        /// Find an item by name
        /// </summary>
        /// <returns>Default if item was not found</returns>
        public async Task<IItem> GetItemByNameAsync(string name, ulong guildID)
        {
            using var _context = new Context(_options);

            //check for item in ordinary items database
            IItem item = await _context.Items.Where(i=> i.GuildID==guildID).FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()).ConfigureAwait(false) ?? null!;
            return item;

        }

        public async Task CreateNewItemAsync(IItem item,ulong guildID, bool isEpicItem=false)
        {
            using var _context = new Context(_options);

           if(!isEpicItem)
              await _context.Items.AddAsync(new ItemBase(guildID, item)).ConfigureAwait(false);
           //else
                //await _context.EpicItems.AddAsync(new EpicItem(item)).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateItemAsync(IItem item, ulong guildID)
        {
            using var _context = new Context(_options);

            _context.Items.Update(new ItemBase(guildID, item));
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<List<IItem>> GetItemsListAsync(ulong guildID)
        {
            using var context = new Context(_options);
            List<IItem> Items = await context.Items.Where(i=> i.GuildID==guildID).Cast<IItem>().ToListAsync().ConfigureAwait(false);

            return Items;
        }

        public async Task<IItem> GetRandomItemAsync(ulong guildID)
        {
            using var _context = new Context(_options);
            var items = _context.Items.Where(i => i.GuildID == guildID);
            int randomIndex = BotMath.RandomNumberGenerator.Next(0, items.Count());

            return await items.Skip(randomIndex).Take(1).FirstAsync();
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
