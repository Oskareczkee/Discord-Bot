using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Web_UI.Models.Filters
{
    public class Filters
    {
        public ulong? GuildID { get; set; }

        [Display(Name = "Name")]
        public string? NameFilter { get; set; } = string.Empty;
        [Display(Name = "Description")]
        public string? DescriptionFilter { get; set; } = string.Empty;

        [Display(Name = "Min Strength")]
        public int? MinStrength { get; set; }
        [Display(Name = "Max Strength")]
        public int? MaxStrength { get; set; }
        [Display(Name = "Min Agility")]
        public int? MinAgility { get; set; }
        [Display(Name = "Max Agility")]
        public int? MaxAgility { get; set; }
        [Display(Name = "Min Intelligence")]
        public int? MinIntelligence { get; set; }
        [Display(Name = "Max Intelligence")]
        public int? MaxIntelligence { get; set; }
        [Display(Name = "Min Endurance")]
        public int? MinEndurance { get; set; }
        [Display(Name = "Max Endurance")]
        public int? MaxEndurance { get; set; }
        [Display(Name = "Min Luck")]
        public int? MinLuck { get; set; }
        [Display(Name = "Max Luck")]
        public int? MaxLuck { get; set; }
    }
}
