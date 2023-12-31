﻿using DB.Models.Profiles;
using static System.Math;

namespace Core.Math
{
    public partial class BotMath
    {
        public static Random RandomNumberGenerator = new Random((int)DateTime.Now.Ticks);
        private static readonly object syncLock = new object();

        /// <summary>
        /// Synchronized random number generation used in functions that are using random multiple times in
        /// small time intervals
        /// </summary>
        public static double SynchronizedRandomDouble(double min, double max)
        {
            lock(syncLock){
                return RandomNumberGenerator.NextDouble() * (max - min) + min;
            }
        }

        /// <summary>
        /// Calculates the cost of levelling up a skill depending on its actual level
        /// </summary>
        /// <param name="skillLevel"></param>
        /// <returns></returns>
        public static ulong SkillLevelUpCost(int skillLevel)
        {
            //casting should work like flooring, we like player, so we offer him always floored price
            return (ulong)(Log(skillLevel) + (Pow(skillLevel, 2) * 0.01));
        }

        /// <summary>
        /// Calculates the full cost of leveling up a skill from starting level to ending level
        /// </summary>
        /// <param name="startingLevel"></param>
        /// <param name="endingLevel"></param>
        /// <returns></returns>
        public static ulong SkillLevelUpCost(int startingLevel, int endingLevel)
        {
            ulong output = 0;

            while(startingLevel<=endingLevel)
            {
                output += SkillLevelUpCost(startingLevel);
                startingLevel++;
            }

            return output;
        }


        //i dont know if this is a good practice to have a so much dependecies in simple calculations
        //maybe i should change it, but for now i will keep it

        /// <summary>
        /// calculates a price of particular skill from a particular profile
        /// </summary>
        /// <param name="skillType"></param>
        /// <param name="profile"></param>
        /// <param name="skillAmount"></param>
        /// <returns></returns>
        public static  ulong calculatePrice(SkillType skillType, Profile profile, int skillAmount)
        {
            //subtract by one, otherwise cost will be calculated to one skill too much
            skillAmount--;

            switch (skillType)
            {
                case SkillType.Strength:
                    return BotMath.SkillLevelUpCost(profile.Strength, profile.Strength + skillAmount);
                case SkillType.Agility:
                    return BotMath.SkillLevelUpCost(profile.Agility, profile.Agility + skillAmount);
                case SkillType.Intelligence:
                    return BotMath.SkillLevelUpCost(profile.Intelligence, profile.Intelligence + skillAmount);
                case SkillType.Endurance:
                    return BotMath.SkillLevelUpCost(profile.Endurance, profile.Endurance + skillAmount);
                case SkillType.Luck:
                    return BotMath.SkillLevelUpCost(profile.Luck, profile.Luck + skillAmount);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Rolls a dice
        /// </summary>
        /// <param name="chance">Chance to roll true (should be in percents [0.25]=25%)</param>
        /// <returns></returns>
        public static bool Roll(double chance)
        {
            double roll = RandomNumberGenerator.NextDouble();
            if (chance >= roll)
                return true;
            return false;
        }

        public static int ShopRerollCost(int level)
        {
            return level * 50;
        }
    }
}
