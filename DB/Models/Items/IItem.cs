using DB.Models.Items.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Models.Items
{
    public interface IItem
    {
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Endurance { get; set; }
        public int Luck { get; set; }
        public int Armor { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public ItemType Type { get; set; }

        public WeaponType WeaponType { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }

        //this property is not used, but has to be implemented as interface requires it
        public string Modifiers { get; set; }

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
    }
}
