using DB.Models.Items;
using DB.Models.Items.Enums;
using DB.Models.Mobs;
/*
 * I put scaling in the math, cause i want to all things related to maths and scaling etc
 * to be in one place, scaling services just uses functions from this library
 */

namespace Core.Math
{
    public partial class BotMath
    {

        public static IItem ScaleItem(IItem item, int level, bool useRandomFactors = true)
        {
            double randomFactor = 0;

            //item can be up to 30% better or 10% worse
            if (useRandomFactors)
                randomFactor = SynchronizedRandomDouble(-0.1, 0.3);

            item.Armor = (int)(item.Armor*(1 + randomFactor) + (level / 10));
            item.Strength = (int)(item.Strength * (1 + randomFactor) + (level / 10));
            item.Agility = (int)(item.Agility * (1 + randomFactor) + (level / 10));
            item.Intelligence = (int)(item.Intelligence * (1 + randomFactor) + (level / 10));
            item.Endurance = (int)(item.Endurance * (1 + randomFactor) + (level / 10));
            item.Luck = (int)(item.Luck * (1 + randomFactor) + (level/10));

            if(item.Type==ItemType.Weapon)
            {
                item.MinDamage = (int)(item.MinDamage * (1 + randomFactor) + (level / 10));
                item.MaxDamage = (int)(item.MaxDamage * (1 + randomFactor) + (level / 10));
            }

            item.Price += (int)(item.Price*((1.5+randomFactor) * level));

            return item;
        }

        public static Mob ScaleMob(Mob mob, int level)
        {
            mob.Strength += (int)(5 * level);
            mob.Agility += (int)(5 * level);
            mob.Intelligence += (int)(5 * level);
            mob.Endurance += (int)(5 * level);
            mob.Luck += level;

            mob.XPAward *= (int)(1.1 * level);
            mob.GoldAward *= (int)(1.2 * level);
            mob.Level = level;

            mob.HP = mob.Endurance *(level + 1) + mob.HP * (level+1)/2;
            mob.BaseDMG = level;
            return mob;
        }
    }
}
