using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Models.Items.Enums;

namespace DB.Models.Items
{
    public class ItemBase : Stats, IItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public ItemType Type { get; set; } = ItemType.None;
        public WeaponType WeaponType { get; set; } = WeaponType.None;
        public int MinDamage { get; set; } = 0;
        public int MaxDamage { get; set; } = 0;
        public string Modifiers { get; set; } = string.Empty;
    }
}
