using System.ComponentModel.DataAnnotations;
using DB.Models.Items.Enums;
using DB.Models.Servers;
using DB.Models.Validation;

namespace DB.Models.Items
{
    public class ItemBase : Stats, IItem
    {
        /*Each item base is unique to server, different servers may have different items*/
        public ulong? GuildID { get; set; }
        public Server? Server { get; set; }

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

        public ItemBase(ulong? guildID, IItem item)
        {
            GuildID = guildID;
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            Type = item.Type;
            WeaponType = item.WeaponType;
            MinDamage = item.MinDamage;
            MaxDamage = item.MaxDamage;
            Modifiers = item.Modifiers;
        }

        public ItemBase() { }
    }
}
