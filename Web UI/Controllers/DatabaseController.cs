using DB;
using DB.Models.Items;
using DB.Models.Mobs;
using Microsoft.AspNetCore.Mvc;
using Web_UI.Models;

namespace Web_UI.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly Context context = null!;
        public DatabaseController(Context ctx) => context = ctx;
        private const int PageItemCount = 15;
        public ViewResult Items(DatabaseViewModel<ItemBase> model)
        {
            model.Entities = context.Items.Take(PageItemCount).ToList();
            return View(model);
        }

        public ViewResult Mobs(DatabaseViewModel<Mob> model)
        {
            model.Entities = context.Mobs.Take(PageItemCount).ToList();
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

        public PartialViewResult DeletePartialView(DeleteViewModel model)
        {
            return PartialView("Views/Database/Partials/_DeletePartial.cshtml", model);
        }

        public PartialViewResult AddItemPartialView(ItemBase item)
        {
            AddItemViewModel model = new() { Item = item };
            return PartialView("Views/Database/Partials/_AddItemPartial.cshtml", model);
        }

        public PartialViewResult AddModifierPartialView(int model)
        {
            return PartialView("Views/Database/Partials/_AddModifierPartial.cshtml", model);
        }

        public PartialViewResult AddMobPartialView(Mob model)
        {
            return PartialView("Views/Database/Partials/_AddMobPartial.cshtml", model);
        }
    }
}
