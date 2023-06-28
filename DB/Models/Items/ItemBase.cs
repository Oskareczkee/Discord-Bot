using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Models.Items.Enums;
using DB.Models.Validation;

namespace DB.Models.Items
{
    public class ItemBase : Stats, IItem
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter a price")]
        [Range(0, int.MaxValue, ErrorMessage ="Price cannot be negative")]
        public double Price { get; set; } = 0;
        public ItemType Type { get; set; } = ItemType.Miscellaneous;
        public WeaponType WeaponType { get; set; } = WeaponType.None;

        [Range(0, int.MaxValue, ErrorMessage = "Min Damage cannot be negative")]
        [NumberLessThan(nameof(this.MaxDamage), ErrorMessage = "Min Damage cannot be greater than Max Damage")]
        public int MinDamage { get; set; } = 0;
        [Range(0, int.MaxValue, ErrorMessage = "Max Damage cannot be negative")]
        public int MaxDamage { get; set; } = 0;
        public string Modifiers { get; set; } = string.Empty;
    }
}
