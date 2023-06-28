using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Models.Items.Enums
{
    public enum ItemType
    {
        Helmet = 1,
        Chestplate = 2,
        Gloves = 3,
        Shoes = 4,
        Weapon = 5,
        Ring = 6,
        Belt = 7,
        Necklace = 8,
        Extra = 9,
        Potion = 10,
        Miscellaneous = 11
    }

    public enum WeaponType
    {
        None = 0,
        Range = 1,
        Melee = 2,
        Magic = 3,
    }

    public enum Modifiers
    {
        MeleeDamage = 1,
        MagicDamage = 2,
        MagicAttackChance = 3,
        CriticalAttackChance = 4,
        CriticalDamage = 5,
        DodgeChance = 6,
        Damage = 7
    }
}
