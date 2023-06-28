using System;
using System.Collections.Generic;
using System.Linq;
using DB.Models.Items.Enums;

namespace DB.Models.Items
{
    public class Modifier
    {
        public Modifiers modifier { get; set; }
        public int BonusPercent { get; set; } = 0;

        public Modifier(Modifiers Modifier, int Value)
        {
            modifier = Modifier;
            BonusPercent = Value;
        }

        public static string GetModifierString(Modifiers modifier)
        {
            return modifier switch
            {
                Modifiers.MeleeDamage => "Melee Damage: ",
                Modifiers.MagicDamage => "Magic Damage: ",
                Modifiers.MagicAttackChance => "Magic Attack Chance: ",
                Modifiers.CriticalAttackChance => "Critical Attack Chance: ",
                Modifiers.CriticalDamage => "Critical Damage: ",
                Modifiers.DodgeChance => "Dodge Chance: ",
                Modifiers.Damage => "Damage: ",
                _ => string.Empty,
            };
        }

        public override string ToString()
        {
            //if bonuspercanet is positive add plus before it
            string plus = string.Empty;
            if (BonusPercent > 0)
                plus = "+";
            return GetModifierString(modifier) + plus + BonusPercent + "%\n";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0018:Deklaracja zmiennej wbudowanej", Justification = "<Oczekujące>")]
        public static List<Modifier> GetModifiersFromString(string modifiersString)
        {
            List<Modifier> output = new();

            var modifiersSplit = modifiersString.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            //check if there are pairs of modifiers
            //if not return empty list
            if (modifiersSplit.Count % 2 != 0 || modifiersSplit.Count == 0)
                return new List<Modifier>();

            //each item can have maximally 3 modifiers, thats why we check max 6 entries
            for (int x = 0; x < 6 && x < modifiersSplit.Count; x += 2)
            {
                int modifierInt;
                if (!int.TryParse(modifiersSplit[x], out modifierInt))
                    return new List<Modifier>();
                int modifierValue;
                if (!int.TryParse(modifiersSplit[x + 1], out modifierValue))
                    return new List<Modifier>();

                try
                {
                    Modifier modifier = new((Modifiers)modifierInt, modifierValue);
                    output.Add(modifier);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new List<Modifier>();
                }

            }

            return output;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0018:Deklaracja zmiennej wbudowanej", Justification = "<Oczekujące>")]
        public static bool CheckModifiersStringValidity(string modifiersString)
        {
            var modifiersSplit = modifiersString.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            //-1 because we do not count None modifier
            int modifiersCount = Enum.GetValues(typeof(Modifiers)).Length - 1;

            if (modifiersSplit.Count % 2 != 0 || modifiersSplit.Count == 0)
                return false;

            for (int x = 0; x < modifiersSplit.Count; x += 2)
            {
                int modifierInt;
                if (!int.TryParse(modifiersSplit[x], out modifierInt))
                    return false;
                if (!int.TryParse(modifiersSplit[x + 1], out _))
                    return false;

                //check if modfier is out of range of enum
                if (modifierInt > modifiersCount || modifierInt < 1)
                    return false;
            }

            return true;
        }
    }
}
