using DB.Models.Items.Enums;
using System.ComponentModel.DataAnnotations;

namespace Web_UI.Models.Filters
{
    public class ItemFilters : Filters
    {
        [Display(Name = "Min Price")]
        public int? MinPrice { get; set; }
        [Display(Name = "Max Price")]
        public int? MaxPrice { get; set; }
        [Display(Name = "Type")]
        public ItemType? TypeFilter { get; set; }
        [Display(Name = "Min Damage")]
        public int? MinDamage { get; set; }
        [Display(Name = "Max Damage")]
        public int? MaxDamage { get; set; }
        [Display(Name = "Min Armor")]
        public int? MinArmor { get; set; }
        [Display(Name = "Max Armor")]
        public int? MaxArmor { get; set; }
        public List<Modifiers>? Modifiers { get; set; } = new();
    }
}
