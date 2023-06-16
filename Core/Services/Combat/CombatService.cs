using DB;
using Microsoft.EntityFrameworkCore;
using Core.Math;
using DB.Models.Profiles;
using DB.Models;
using DB.Models.Items.Enums;
using DB.Models.Mobs;

namespace Core.Services.Combat
{

    public enum AttackType
    {
        Dodge=1,
        Attack=2,
        Critical=3,
        Magic=4,
        CriticalMagic=5
    }
    public class CombatInfo
    {
        public int Damage = 0;
        //since hp is readonly we have to store damage
        public AttackType attackType;
        
    }

    public class CombatProfileAndDamage
    {
        //profile and mob is set to null to set them manually
        //this allows use of this class in quest without much more added code
        public Entity profile { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DamageTaken { get; set; } = 0;
        public ulong DiscordID { get; set; } = 0;
        public ulong GuildID { get; set; } = 0;
        public bool isMob { get; set; } = false;

        public Dictionary<Modifiers, double> modifiers { get; set; } = new Dictionary<Modifiers, double>();
        public WeaponType WeaponType { get; set; } = WeaponType.None;
        public (int min, int max) minMaxWeaponDMG { get; set; } = new(0, 0);
    }

    public interface ICombatService
    {
        CombatInfo Attack(CombatProfileAndDamage attacker, CombatProfileAndDamage defender);
        //CombatInfo Attack(IProfile attacker, IProfile defender);
    }


    public class CombatService : ICombatService
    {
        private readonly DbContextOptions<Context> _options;

        public CombatService(DbContextOptions<Context> options)
        {
            _options = options;
        }

        public CombatInfo Attack(CombatProfileAndDamage Attacker, CombatProfileAndDamage Defender)
        {
            CombatInfo outputInfo = new CombatInfo();

            bool criticalHit = false;
            bool magick = false;

            double attackerDamageMultiplier = BotMath.RandomNumberGenerator.NextDouble()+0.5;
            double defenderDodgeChance = BotMath.CalculateDodgeChance(Defender.profile.Agility, Attacker.profile.Strength, 
                                        Attacker.modifiers.GetValueOrDefault(Modifiers.DodgeChance));

            if(BotMath.Roll(defenderDodgeChance))
            {
                outputInfo.attackType = AttackType.Dodge;
                return outputInfo;
            }

            //if not dodged FIGHT
            //roll critical attack
            double criticalHitChance = BotMath.CalculateCritChance(Attacker.profile.Luck, Defender.profile.Level, 
                                       Attacker.modifiers.GetValueOrDefault(Modifiers.CriticalAttackChance));

            if (BotMath.Roll(criticalHitChance))
            {
                attackerDamageMultiplier *= 2*(1 + Attacker.modifiers.GetValueOrDefault(Modifiers.CriticalDamage));
                criticalHit = true;
            }

            double magickAttackChance = BotMath.CalculateMagickChance(Attacker.profile.Luck, Attacker.profile.Intelligence, 
                                        Attacker.modifiers.GetValueOrDefault(Modifiers.MagicAttackChance));
            if(BotMath.Roll(magickAttackChance))
            {
                attackerDamageMultiplier *= BotMath.CalculateMagickAttackBonus(Attacker.profile.Level, Defender.profile.Intelligence, 
                                            Attacker.modifiers.GetValueOrDefault(Modifiers.MagicDamage));
                magick = true;
            }

            double resistance = 0;
            //mobs dont have armor but overall resistance
            if (Defender.isMob)
                resistance = ((Mob)Defender.profile).Resistance / 100;
            else
                resistance = BotMath.CalculateDamageResistance(Defender.profile.Level, Defender.profile.Armor);

            int weaponDMG = 0;

            switch (Attacker.WeaponType)
            {
                case WeaponType.None:
                    weaponDMG = Attacker.profile.BaseDMG;
                    break;
                case WeaponType.Range:
                    weaponDMG = BotMath.CalculateWeaponDamage(Attacker.minMaxWeaponDMG.min, Attacker.minMaxWeaponDMG.max, Attacker.profile.Agility);
                    break;
                case WeaponType.Melee:
                    weaponDMG = BotMath.CalculateWeaponDamage(Attacker.minMaxWeaponDMG.min, Attacker.minMaxWeaponDMG.max, Attacker.profile.Strength);
                    break;
                case WeaponType.Magic:
                    weaponDMG = BotMath.CalculateWeaponDamage(Attacker.minMaxWeaponDMG.min, Attacker.minMaxWeaponDMG.max, Attacker.profile.Intelligence);
                    break;
                default:
                    break;
            }

            int Damage = BotMath.CalculateDamage(Attacker.profile.Strength, weaponDMG, attackerDamageMultiplier, resistance, 
                         Attacker.modifiers.GetValueOrDefault(Modifiers.Damage));
            outputInfo.Damage = Damage;

            if (criticalHit && magick)
                outputInfo.attackType = AttackType.CriticalMagic;
            else if (criticalHit)
                outputInfo.attackType = AttackType.Critical;
            else if (magick)
                outputInfo.attackType = AttackType.Magic;
            else
                outputInfo.attackType = AttackType.Attack;


            return outputInfo;
        }
    }
}
