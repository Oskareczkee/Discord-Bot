using DB;
using DB.Models.Items;
using DB.Models.Mobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_UI.Models;

namespace Web_UI.Controllers
{
    [Authorize]
    public partial class DatabaseController : Controller
    {
        private readonly Context context = null!;
        public DatabaseController(Context ctx) => context = ctx;
        private const int PageItemCount = 15;

        public ViewResult Items(DatabaseViewModel<ItemBase> model)
        {
            var session = new DatabaseSession(HttpContext.Session);
            var filters = session.GetItemFilters();
            filters.GuildID = session.GetServer();

            var items = FilterItems(filters).ToList();
            model.Entities = items.Take(PageItemCount).ToList();
            return View(model);
        }

        public ViewResult Mobs(DatabaseViewModel<Mob> model)
        {
            var session = new DatabaseSession(HttpContext.Session);
            var filters = session.GetMobFilters();
            filters.GuildID = session.GetServer();

            var mobs = FilterMobs(filters).ToList();
            model.Entities = mobs.Take(PageItemCount).ToList();
            return View(model);
        }

        public RedirectToActionResult DeleteItem(int id)
        {
            var item = context.Items.Find(id);
            context.Remove<ItemBase>(item!);
            context.SaveChanges();

            return RedirectToAction("Items");

        }

        public RedirectToActionResult DeleteMob(int id)
        {
            var mob = context.Mobs.Find(id);
            context.Remove(mob!);
            context.SaveChanges();

            return RedirectToAction("Mobs");
        }

        [HttpPost]
        public IActionResult AddItem(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                string modifiers = string.Join(' ', model.Modifiers);
                model.Item.Modifiers = modifiers;
                var guildID = new DatabaseSession(HttpContext.Session).GetServer();
                model.Item.GuildID = guildID;
                context.Items.Update(model.Item);
                context.SaveChanges();
                return RedirectToAction("Items");
            }

            var Session = new DatabaseSession(HttpContext.Session);
            var errors = ModelState.Values.SelectMany(e => e.Errors.Select(me => me.ErrorMessage)).ToList();
            string errorMessage = $"There were some errors during Add/Update of {model.Item.Name}";
            Session.AddError(new Error { Description = errorMessage, Errors=errors});
            return RedirectToAction("Items");
        }

        [HttpPost]
        public IActionResult AddMob(Mob model)
        {
            if(ModelState.IsValid)
            {
                var guildID = new DatabaseSession(HttpContext.Session).GetServer();
                model.GuildID = guildID;
                context.Mobs.Update(model);
                context.SaveChanges();
                return RedirectToAction("Mobs");
            }

            var Session = new DatabaseSession(HttpContext.Session);
            var errors = ModelState.Values.SelectMany(e => e.Errors.Select(me => me.ErrorMessage)).ToList();
            string errorMessage = $"There were some errors during Add/Update of {model.Name}";
            Session.AddError(new Error { Description = errorMessage, Errors = errors });
            return RedirectToAction("Mobs");
        }
    }
}
