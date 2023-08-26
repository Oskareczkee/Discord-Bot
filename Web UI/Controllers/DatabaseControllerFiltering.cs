using DB.Models.Items;
using DB.Models.Mobs;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_UI.Models;
using Web_UI.Models.Filters;

namespace Web_UI.Controllers
{
    public partial class DatabaseController
    {
        public RedirectToActionResult ChangeServer(ulong id, string redirectAction, string redirectController)
        {
            var session = new DatabaseSession(HttpContext.Session);
            session.SetServer(id);
            return RedirectToAction(redirectAction, redirectController); //refresh page, couldn't find better way to do this, but i believe there is a better way
        }

        public RedirectToActionResult SaveItemFilters(ItemFilters filters)
        {
            var session = new DatabaseSession(HttpContext.Session);
            session.SetItemFilters(filters);
            return RedirectToAction("Items");
        }

        public RedirectToActionResult SaveMobFilters(MobFilters filters)
        {
            var session = new DatabaseSession(HttpContext.Session);
            session.SetMobFilters(filters);
            return RedirectToAction("Mobs");
        }

        public RedirectToActionResult ClearItemFilters()
        {
            var session = new DatabaseSession(HttpContext.Session);
            session.SetItemFilters(new ItemFilters());
            return RedirectToAction("Items");
        }

        public RedirectToActionResult ClearMobFilters()
        {
            var session = new DatabaseSession(HttpContext.Session);
            session.SetMobFilters(new MobFilters());
            return RedirectToAction("Mobs");
        }

        private IQueryable<Mob> FilterMobs(MobFilters filter)
        {
            IQueryable<Mob> query = context.Mobs;

            query = query.Where(t => t.GuildID == filter.GuildID); //if guild is null, query will not have records

            if (!string.IsNullOrEmpty(filter.NameFilter))
                query = query.Where(t => t.Name.Contains(filter.NameFilter));
            if (!string.IsNullOrEmpty(filter.DescriptionFilter))
                query = query.Where(t => t.Description.Contains(filter.DescriptionFilter));


            if (filter.MinStrength != null)
                query = query.Where(t => t.Strength >= filter.MinStrength);
            if (filter.MaxStrength != null)
                query = query.Where(t => t.Strength <= filter.MaxStrength);
            if (filter.MinAgility != null)
                query = query.Where(t => t.Agility >= filter.MinAgility);
            if (filter.MaxAgility != null)
                query = query.Where(t => t.Agility <= filter.MaxAgility);
            if (filter.MinIntelligence != null)
                query = query.Where(t => t.Intelligence >= filter.MinIntelligence);
            if (filter.MaxIntelligence != null)
                query = query.Where(t => t.Intelligence <= filter.MaxIntelligence);
            if (filter.MinEndurance != null)
                query = query.Where(t => t.Endurance >= filter.MinEndurance);
            if (filter.MaxEndurance != null)
                query = query.Where(t => t.Endurance <= filter.MaxEndurance);
            if (filter.MinLuck != null)
                query = query.Where(t => t.Luck >= filter.MinLuck);
            if (filter.MaxLuck != null)
                query = query.Where(t => t.Luck <= filter.MaxLuck);

            if (filter.MinGoldAward != null)
                query = query.Where(t => t.GoldAward >= filter.MinGoldAward);
            if (filter.MaxGoldAward != null)
                query = query.Where(t => t.GoldAward <= filter.MaxGoldAward);
            if (filter.MinXPAward != null)
                query = query.Where(t => t.XPAward >= filter.MinXPAward);
            if (filter.MaxXPAward != null)
                query = query.Where(t => t.XPAward <= filter.MaxXPAward);

            if (filter.MinResistance != null)
                query = query.Where(t => t.Resistance >= filter.MinResistance);
            if (filter.MaxResistance != null)
                query = query.Where(t => t.Resistance <= filter.MaxResistance);

            if (filter.MinHP != null)
                query = query.Where(t => t.HP >= filter.MinHP);
            if (filter.MaxHP != null)
                query = query.Where(t => t.HP <= filter.MaxHP);
            if (filter.MinDMG != null)
                query = query.Where(t => t.BaseDMG >= filter.MinDMG);
            if (filter.MaxHP != null)
                query = query.Where(t => t.BaseDMG <= filter.MaxDMG);

            return query;
        }

        private IQueryable<ItemBase> FilterItems(ItemFilters filter)
        {
            IQueryable<ItemBase> query = context.Items;

            query = query.Where(t => t.GuildID == filter.GuildID); //if guild is null, query will not have records

            if (filter.TypeFilter != null)
                query = query.Where(t => t.Type == filter.TypeFilter);
         
            if (!string.IsNullOrEmpty(filter.NameFilter))
                query = query.Where(t => t.Name.Contains(filter.NameFilter));
            if (!string.IsNullOrEmpty(filter.DescriptionFilter))
                query = query.Where(t => t.Description.Contains(filter.DescriptionFilter));


            if (filter.MinStrength != null)
                query = query.Where(t => t.Strength >= filter.MinStrength);
            if (filter.MaxStrength != null)
                query = query.Where(t => t.Strength <= filter.MaxStrength);
            if (filter.MinAgility!= null)
                query = query.Where(t => t.Agility >= filter.MinAgility);
            if (filter.MaxAgility != null)
                query = query.Where(t => t.Agility <= filter.MaxAgility);
            if (filter.MinIntelligence != null)
                query = query.Where(t => t.Intelligence >= filter.MinIntelligence);
            if (filter.MaxIntelligence != null)
                query = query.Where(t => t.Intelligence <= filter.MaxIntelligence);
            if (filter.MinEndurance != null)
                query = query.Where(t => t.Endurance >= filter.MinEndurance);
            if (filter.MaxEndurance != null)
                query = query.Where(t => t.Endurance <= filter.MaxEndurance);
            if (filter.MinLuck != null)
                query = query.Where(t => t.Luck >= filter.MinLuck);
            if (filter.MaxLuck != null)
                query = query.Where(t => t.Luck <= filter.MaxLuck);
            if (filter.MinArmor != null)
                query = query.Where(t => t.Armor >= filter.MinArmor);
            if (filter.MaxArmor != null)
                query = query.Where(t => t.Armor <= filter.MaxArmor);

            if (filter.MinPrice != null)
                query = query.Where(t => t.Price >= filter.MinPrice);
            if (filter.MaxPrice != null)
                query = query.Where(t => t.Price <= filter.MaxPrice);

            if (filter.MinDamage != null)
                query = query.Where(t => t.MinDamage>= filter.MinDamage);
            if (filter.MaxDamage != null)
                query = query.Where(t => t.MaxDamage <= filter.MaxDamage);

            return query;
        }
    }
}
