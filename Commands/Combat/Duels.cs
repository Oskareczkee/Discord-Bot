using Bot.Attributes;
using Core.Math;
using Core.Services.Combat;
using Core.Services.Items;
using Core.Services.Mobs;
using Core.Services.Profiles;
using DB.Models;
using DB.Models.Items;
using DB.Models.Profiles;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bot.Commands.Combat
{
    public partial class DBCombatCommands : BaseCommandModule
    {
        private readonly ICombatService _combatService;
        private readonly IProfileService _profileService;
        private readonly IExperienceService _expService;
        private readonly IMobService _mobService;
        private readonly IItemService _itemService;
        public DBCombatCommands(ICombatService combatService, IProfileService profileService, IExperienceService expService, IMobService mobService, IItemService itemService)
        {
            _combatService = combatService;
            _profileService = profileService;
            _expService = expService;
            _mobService = mobService;
            _itemService = itemService;
        }

        private async Task<CombatProfileAndDamage> CreatePlayerProfileAndDamage(ulong discordID, ulong guildID, string Name)
        {
            CombatProfileAndDamage output = new CombatProfileAndDamage();

            Profile Profile = await _profileService.GetProfileWithEquipmentStats(discordID, guildID).ConfigureAwait(false);

            output.profile = Profile;
            output.DiscordID = discordID;
            output.GuildID = guildID;
            output.Name = Name;

            output.modifiers = await _profileService.GetEquipmentModifiers(discordID, guildID).ConfigureAwait(false);

            Item weapon = Profile.Equipment[4];

            output.WeaponType = weapon.WeaponType;
            output.minMaxWeaponDMG = (weapon.MinDamage, weapon.MaxDamage);

            return output;
        }

        [Command("fight")]
        [Description("Become the master of arena!")]
        [CheckTime(TimeCheckMode.Fight, 600)]
        public async Task Fight(CommandContext ctx, DiscordMember mention = null)
        {
            if (mention == null || mention == ctx.Member)
            {
                await ctx.Channel.SendMessageAsync("Bruh");
                await ctx.Channel.SendMessageAsync($"{ctx.Member.DisplayName} wants to duel himself!");
                await ctx.Channel.SendMessageAsync("\"I'll take him off with that rope\" he thinks");
                await ctx.Channel.SendMessageAsync("He found a tree and hanged himself on a tree");
                await ctx.Channel.SendMessageAsync("What a shame !");
                return;
            }

            CombatProfileAndDamage attacker = await CreatePlayerProfileAndDamage(ctx.Member.Id, ctx.Guild.Id, ctx.Member.DisplayName).ConfigureAwait(false);
            CombatProfileAndDamage defender = await CreatePlayerProfileAndDamage(mention.Id, mention.Guild.Id, mention.DisplayName).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync($"{ctx.Member.DisplayName} will duel {mention.DisplayName} !");

            CombatInfo combatInfo;

            //since hp is readonly we have to store amount of damage dealt and compare it to the duelers HPs
            while (true)
            {
                combatInfo = _combatService.Attack(attacker, defender);
                switch (combatInfo.attackType)
                {
                    case AttackType.Dodge:
                        await ctx.Channel.SendMessageAsync($"{defender.Name} dodged {attacker.Name}'s attack !");
                        break;
                    case AttackType.Attack:
                        await ctx.Channel.SendMessageAsync($"{attacker.Name} hits {defender.Name} for {combatInfo.Damage} HP !");
                        break;
                    case AttackType.Critical:
                        await ctx.Channel.SendMessageAsync($"{attacker.Name} critically stabbed {defender.Name} knee for {combatInfo.Damage} HP !");
                        break;
                    case AttackType.Magic:
                        await ctx.Channel.SendMessageAsync($"{attacker.Name} mumbled something stupid and casted a fireball on {defender.Name} for {combatInfo.Damage} HP !");
                        break;
                    case AttackType.CriticalMagic:
                        await ctx.Channel.SendMessageAsync($"{attacker.Name} got lucky and guessed powerful spell and threw it at {defender.Name} for {combatInfo.Damage} HP !");
                        break;
                    default:
                        await ctx.Channel.SendMessageAsync("Combat Commands fight - undefined attack type");
                        return;
                }
                defender.DamageTaken += combatInfo.Damage;
                if (defender.DamageTaken >= defender.profile.HP)
                {
                    await ctx.Channel.SendMessageAsync($"{attacker.Name} brutally finished off {defender.Name} making him swim in pool of his own blood !");
                    int GoldAmount = BotMath.CalculateGoldAmount(defender.profile.Level, attacker.profile.Luck, attacker.profile.Level);
                    int XPAmount = BotMath.CalculateXPAmount(defender.profile.Level, attacker.profile.Luck, attacker.profile.Level);
                    await ctx.Channel.SendMessageAsync($"{attacker.Name} was awarded with {GoldAmount} gold and {XPAmount} experience points !");

                    await _profileService.AddGold(attacker.DiscordID, attacker.GuildID, GoldAmount);
                    await _expService.GrantXPAsync(attacker.DiscordID, attacker.GuildID, XPAmount);
                    break;
                }

                //swap
                (attacker, defender) = (defender, attacker);
            }
        }

    }
}
