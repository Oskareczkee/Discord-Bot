using System;
using System.Linq;
using DB.Models.Items.Enums;

/*
   Item might look pretty weird, especially ProfileItem
   EF Core forces links between types, so when i add an Item to ProfileItem
   it forces it to link that Item to the Item database
   I didn't want items to be linked, i wanted to players to have multiple same items in their inventory
   As well i wanted to same items have different stats depending on level etc. EF Core did not allow me to do so
   So that's why ProfileItem is pretty much item but with different name
 */

namespace DB.Models.Items
{
    public class Item : Stats, IItem
    {

        public Item()
        {
            Name = "None";
            Description = "None";
            Price = 0;
            Type = ItemType.Miscellaneous;
        }

        //constructor for profile service to add equipment
        public Item(ItemType type)
        {
            Name = "None";
            Description = "None";
            Price = 0;
            Type = type;
        }

        public Item(IItem item)
        {
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            Type = item.Type;

            Strength = item.Strength;
            Agility = item.Agility;
            Intelligence = item.Intelligence;
            Endurance = item.Endurance;
            Luck = item.Luck;
            Armor = item.Armor;

            WeaponType = item.WeaponType;
            MinDamage = item.MinDamage;
            MaxDamage = item.MaxDamage;

            Modifiers = item.Modifiers;
        }

        public void ChangeItemProperties(IItem item)
        {
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            Type = item.Type;

            Strength = item.Strength;
            Agility = item.Agility;
            Intelligence = item.Intelligence;
            Endurance = item.Endurance;
            Luck = item.Luck;
            Armor = item.Armor;
            Modifiers = item.Modifiers;
        }
        public string StatsString()
        {
            if (Type == ItemType.Potion || Type == ItemType.Miscellaneous)
                return "{No stats :(}";

            string output = "{" + $"Armor: {Armor} S: {Strength} A: {Agility} I: {Intelligence} E: {Endurance} L: {Luck}" + "}";

            if (Type == ItemType.Weapon)
                output += $"\n" + $"MinDMG: {MinDamage}\nMaxDMG: {MaxDamage}";

            return output;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0018:Deklaracja zmiennej wbudowanej", Justification = "<Oczekujące>")]
        public string ModifiersString()
        {
            string output = string.Empty;

            var modifiersSplit = Modifiers.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            //check if there are pairs of modifiers
            //if not return empty string
            if (modifiersSplit.Count % 2 != 0)
                return string.Empty;
            if (modifiersSplit.Count == 0)
                return "None";

            //each item can have maximally 3 modifiers, thats why we check max 6 entries
            for (int x = 0; x < 6 && x < modifiersSplit.Count; x += 2)
            {
                int modifierInt;
                if (!int.TryParse(modifiersSplit[x], out modifierInt))
                    return string.Empty;
                int modifierValue;
                if (!int.TryParse(modifiersSplit[x + 1], out modifierValue))
                    return string.Empty;

                try
                {
                    Modifier modifier = new((Modifiers)modifierInt, modifierValue);
                    output += modifier;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return string.Empty;
                }
            }

            return output;
        }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public ItemType Type { get; set; } = ItemType.Miscellaneous;

        public WeaponType WeaponType { get; set; } = WeaponType.None;
        public int MinDamage { get; set; } = 0;
        public int MaxDamage { get; set; } = 0;

        //this property is not used, but has to be implemented as interface requires it
        public string Modifiers { get; set; } = string.Empty;
    }
}
