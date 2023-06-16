using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Math
{
    public partial class BotMath
    {
        /// <summary>
        /// Calulates dodge chance
        /// </summary>
        /// <param name="defenderAgility"></param>
        /// <param name="attackerStrength"></param>
        /// <returns>Chance for dodge, max chance is 50%</returns>
        public static double CalculateDodgeChance(int defenderAgility, int attackerStrength, double DodgeModifier = 0)
        {
            double output = ((defenderAgility * 1.25) - attackerStrength) * 0.01 * (1 + DodgeModifier);
            //maximally 50% chance for dodge
            if (output > 0.5)
                output = 0.5;
            return output;
        }

        /// <summary>
        /// Calculates critical hit chance
        /// </summary>
        /// <param name="attackerLuck"></param>
        /// <param name="defenderLevel"></param>
        /// <returns>Chance for critical hit, max chance is 50%</returns>
        public static double CalculateCritChance(int attackerLuck, int defenderLevel, double CritChanceModifier = 0)
        {
            double output = (attackerLuck * 5 / (defenderLevel * 2)) * 0.01 * (1 + CritChanceModifier);
            //maximally 50% chance for critical hit
            if (output > 0.5)
                output = 0.5;
            return output;
        }

        /// <summary>
        /// Calculates magic attack chance
        /// </summary>
        /// <param name="attackerLuck"></param>
        /// <param name="attackerIntelligence"></param>
        /// <returns>Returns chance for magick attack, max chance is 50%</returns>
        public static double CalculateMagickChance(int attackerLuck, int attackerIntelligence, double MagicChanceModifier = 0)
        {
            double output = (attackerLuck + attackerIntelligence) * 0.01 * (1 + MagicChanceModifier);
            //maximally 50% chance for magic attack
            if (output > 0.5)
                output = 0.5;
            return output;
        }

        /// <summary>
        /// Calculates amount of gold awarded for fight
        /// </summary>
        /// <param name="defenderLevel"></param>
        /// <param name="attackerLuck">Gives higher chance for better drop</param>
        /// <returns>Amount of gold</returns>
        public static int CalculateGoldAmount(int defenderLevel, int attackerLuck, int attackerLevel)
        {
            int output = defenderLevel * 10;
            double bonus = (RandomNumberGenerator.NextDouble() + 1) * attackerLuck / (attackerLevel * 10);
            return (int)(output * bonus);
        }

        public static int CalculateMobGoldAmount(int mobGoldAward, int attackerLuck, int attackerLevel)
        {
            int output = mobGoldAward * 3;
            double bonus = (RandomNumberGenerator.NextDouble() + 1) * attackerLuck / (attackerLevel * 10);
            return (int)(output * bonus);
        }

        /// <summary>
        /// Calculates amount of XP awarded for fight
        /// </summary>
        /// <param name="defenderLevel"></param>
        /// <param name="attackerLuck">Gives higher chance for better drop</param>
        /// <param name="attackerLevel"></param>
        /// <returns></returns>
        public static int CalculateXPAmount(int defenderLevel, int attackerLuck, int attackerLevel)
        {
            int output = defenderLevel * 20;
            double bonus = (RandomNumberGenerator.NextDouble() + 1) * attackerLuck / (attackerLevel * 10);
            return (int)(output * bonus);
        }

        public static int CalculateMobXPAmount(int mobXPAward, int attackerLuck, int attackerLevel)
        {
            int output = mobXPAward * 2;
            double bonus = (RandomNumberGenerator.NextDouble() + 1) * attackerLuck / (attackerLevel * 10);
            return (int)(output * bonus);
        }

        /// <summary>
        /// Calculates bonus damage for magick attack, lowering it if defender has high intelligence
        /// </summary>
        /// <param name="attackerLevel"></param>
        /// <param name="defenderIntelligence"></param>
        /// <returns>Attack bonus, max is 50% more damage</returns>
        public static double CalculateMagickAttackBonus(int attackerLevel, int defenderIntelligence, double MagicDamageModifier = 0)
        {
            double output = ((4 * attackerLevel) / defenderIntelligence) * (1 + MagicDamageModifier);
            if (output > 0.5)
                output = 0.5;

            return 1 + output;
        }

        public static int CalculateDamage(int attackerStrength, int attackerWeaponDMG, double attackerDamageMultiplier, double defenderResistance = 0, double DamageModifier = 0)
        {
            int Damage = (int)(System.Math.Ceiling(attackerDamageMultiplier * attackerWeaponDMG) * (1 + DamageModifier) + attackerStrength*1.5);
            return (int)(Damage * (1 - defenderResistance));

        }

        public static double CalculateDamageResistance(int defenferLevel, int totalArmor)
        {
            double resistance = (totalArmor / defenferLevel);

            //max resistance for player is 50%
            if (resistance > 50)
                resistance = 50;

            //convert to percents and return
            return resistance / 100;
        }

        public static int CalculateWeaponDamage(int weaponMinDMG, int weaponMaxDMG, int weaponSkill)
        {
            int randomDMG = RandomNumberGenerator.Next(weaponMinDMG, weaponMaxDMG + 1);

            return randomDMG * (weaponSkill / 20);
        }
    }
}
