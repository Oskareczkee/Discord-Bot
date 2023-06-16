using Bot.Attributes;
using Core.Math;
using Core.Services.Combat;
using DB.Models.Items;
using DB.Models.Mobs;
using DB.Models.Profiles;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bot.Commands.Combat
{
    public partial class DBCombatCommands : BaseCommandModule
    {
        [Command("quest")]
        [Description("Rip and tear, until it is done.")]
        [CheckTime(TimeCheckMode.Quest, 900)]
        public async Task Quest(CommandContext ctx)
        {
            Profile Profile = await _profileService.GetOrCreateProfileAsync(ctx.Member.Id, ctx.Guild.Id);
            //we set name to bypass problem with names in this function
            CombatProfileAndDamage attacker = await CreatePlayerProfileAndDamage(ctx.Member.Id, ctx.Guild.Id, ctx.Member.DisplayName).ConfigureAwait(false);

            Mob Mob = await _mobService.GetRandomMob();
            //mob should be on a player level or 1 level higher
            int mobLevel = BotMath.RandomNumberGenerator.Next(Profile.Level, Profile.Level + 1);
            Mob = _mobService.ScaleMob(Mob, mobLevel);

            CombatProfileAndDamage defender = new() { profile = Mob, Name = Mob.Name, isMob = true };

            await ctx.Channel.SendMessageAsync($"{defender.Name} is staring at {attacker.Name}! Treasure won't come as easy as you thought...");

            CombatInfo combatInfo;

            await _profileService.ClearPotions(attacker.DiscordID, attacker.GuildID);
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
                    if (attacker.isMob)
                    {
                        await ctx.Channel.SendMessageAsync($"{attacker.Name} brutally chopped off {defender.Name}'s head and impaled it!");
                        await ctx.Channel.SendMessageAsync($"Your eyes are crying with red warm blood");
                        break;
                    }

                    await ctx.Channel.SendMessageAsync($"{attacker.Name} shredded {defender.Name} into little pieces! You cut the head of the beast and kick it like a ball");
                    int GoldAmount = BotMath.CalculateMobGoldAmount(Mob.GoldAward, attacker.profile.Luck, attacker.profile.Level);
                    int XPAmount = BotMath.CalculateMobXPAmount(Mob.XPAward, attacker.profile.Luck, attacker.profile.Level);
                    await ctx.Channel.SendMessageAsync($"{attacker.Name} was awarded with {GoldAmount} gold and {XPAmount} experience points !");

                    await _profileService.AddGold(attacker.DiscordID, attacker.GuildID, GoldAmount);
                    await _expService.GrantXPAsync(attacker.DiscordID, attacker.GuildID, XPAmount);
                    if (BotMath.Roll(0.25))
                    {
                        IItem item = await _itemService.GetRandomItem();
                        await ctx.Channel.SendMessageAsync($"It looks like this monster was snitching something in its place! {item.Name} was added to your inventory!");
                        await _itemService.AddItemAsync(ctx.Member.Id, ctx.Guild.Id, item);
                    }

                    break;
                }

                //swap
                (attacker, defender) = (defender, attacker);
            }
        }
    }
}
