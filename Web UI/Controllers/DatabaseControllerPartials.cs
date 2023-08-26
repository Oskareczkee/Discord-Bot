using DB.Models.Items;
using DB.Models.Mobs;
using Microsoft.AspNetCore.Mvc;
using Web_UI.Models.Filters;
using Web_UI.Models;

namespace Web_UI.Controllers
{
    public partial class DatabaseController
    {
        public PartialViewResult DeletePartialView(DeleteViewModel model)
        {
            return PartialView("Views/Database/Partials/_DeletePartial.cshtml", model);
        }

        public PartialViewResult AddItemPartialView(ItemBase item)
        {
            AddItemViewModel model = new() { Item = item };
            return PartialView("Views/Database/Partials/_AddItemPartial.cshtml", model);
        }

        public PartialViewResult ItemFiltersPartialView(ItemFilters filter)
        {
            return PartialView("Views/Database/Partials/_ItemFiltersPartial.cshtml", filter);
        }

        public PartialViewResult MobFiltersPartialView(MobFilters filter)
        {
            return PartialView("Views/Database/Partials/_MobFiltersPartial.cshtml", filter);
        }

        public PartialViewResult AddModifierFilterPartialView(int model)
        {
            return PartialView("Views/Database/Partials/_AddModifierFilterPartial.cshtml", model);
        }

        public PartialViewResult AddModifierPartialView(int model)
        {
            return PartialView("Views/Database/Partials/_AddModifierPartial.cshtml", model);
        }

        public PartialViewResult AddMobPartialView(Mob model)
        {
            return PartialView("Views/Database/Partials/_AddMobPartial.cshtml", model);
        }

        public PartialViewResult ChangeServerPartialView(RedirectionData model)
        {
            return PartialView("Views/Database/Partials/_ChangeServerPartial.cshtml", model);
        }
    }
}
