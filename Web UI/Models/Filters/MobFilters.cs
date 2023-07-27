
using System.ComponentModel.DataAnnotations;

namespace Web_UI.Models.Filters
{
    public class MobFilters : Filters
    {
        [Display(Name ="Min Gold")]
        public int? MinGoldAward { get; set; }
        [Display(Name = "Max Gold")]
        public int? MaxGoldAward { get; set; }
        [Display(Name = "Min XP")]
        public int? MinXPAward { get; set; }
        [Display(Name ="Max XP")]
        public int? MaxXPAward { get; set; }
        [Display(Name = "Min Resistance %")]
        public int? MinResistance { get; set; }
        [Display(Name ="Max Resistance %")]
        public int? MaxResistance { get; set; }

        [Display(Name = "Min HP")]
        public int? MinHP { get; set; }
        [Display(Name = "Max HP")]
        public int? MaxHP { get; set; }
        [Display(Name = "Min DMG")]
        public int? MinDMG { get; set; }
        [Display(Name = "Max DMG")]
        public int? MaxDMG { get; set; }
    }
}
