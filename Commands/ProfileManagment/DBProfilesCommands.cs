using Bot.Attributes;
using Core.Math;
using Core.Services.Items;
using Core.Services.Profiles;
using DB.Models.Profiles;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bot.Commands.ProfileManagment
{
    public partial class DBProfileCommands : BaseCommandModule
    {
        //the database
        private readonly IProfileService _profileService;
        private readonly IItemService _itemService;
        private readonly IExperienceService _expService;

        public DBProfileCommands(IProfileService ProfileService, IExperienceService expService, IItemService itemService)
        {
            _profileService = ProfileService;
            _expService = expService;
            _itemService = itemService;
        }

        [Command("profile"), AddXP(10)]
        [Description("Shows user profile")]
        public async Task GetProfile(CommandContext ctx, DiscordMember? member = null)
        {
            if (member == null)
                await DisplayProfileAsync(ctx, ctx.Member!.Id).ConfigureAwait(false);
            else
                await DisplayProfileAsync(ctx, member.Id).ConfigureAwait(false);
        }

        private async Task DisplayProfileAsync(CommandContext ctx, ulong memberID)
        {

            Profile profile = await _profileService.GetOrCreateProfileAsync(memberID, ctx.Guild.Id).ConfigureAwait(false);

            DiscordMember member = ctx.Guild.Members[profile.DiscordID];


            var profileEmbed = new DiscordEmbedBuilder
            {
                Title = $"{ctx.Guild.Members[profile.DiscordID].DisplayName}'s profile",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = member.AvatarUrl },
                Color = DiscordColor.Aquamarine
            };

            var eqStats = await _profileService.GetEquipmentStats(memberID, ctx.Guild.Id).ConfigureAwait(false);

            profileEmbed.AddField("XP:", profile.XP.ToString(), true);
            profileEmbed.AddField("Level:", profile.Level.ToString(), true);
            profileEmbed.AddField("Next Level:", profile.NextLevel.ToString(), true);
            profileEmbed.AddField("Gold:", profile.Gold.ToString());
            profileEmbed.AddField("HP:", profile.HP.ToString());
            profileEmbed.AddField("Strength: ", (profile.Strength + eqStats.Strength).ToString() + " Eq(" + eqStats.Strength + ")", true);
            profileEmbed.AddField("Agility: ", (profile.Agility + eqStats.Agility).ToString() + " Eq(" + eqStats.Agility + ")", true);
            profileEmbed.AddField("Intelligence: ", (profile.Intelligence + eqStats.Intelligence).ToString() + " Eq(" + eqStats.Intelligence + ")", true);
            profileEmbed.AddField("Endurance: ", (profile.Endurance + eqStats.Endurance).ToString() + " Eq(" + eqStats.Endurance + ")", true);
            profileEmbed.AddField("Luck: ", (profile.Luck + eqStats.Luck).ToString() + " Eq(" + eqStats.Luck + ")", true);
            //basically armor can come only from equipment
            profileEmbed.AddField("Armor:", (profile.Armor + eqStats.Armor).ToString() + " Eq(" + eqStats.Armor + ")", true);

            if (profile.Items.Count > 0)
                profileEmbed.AddField("Items:", string.Join(", ", profile.Items.Select(x => x.Name)));
            else
                profileEmbed.AddField("Items:", "None");


            await ctx.Channel.SendMessageAsync(embed: profileEmbed).ConfigureAwait(false);
        }

        //i didn't know where to put this command, so i left it here
        [Command("beggold"), AddXP(5)]
        [Description("Beggar simulator")]
        public async Task BegGold(CommandContext ctx)
        {
            int GoldAmount = BotMath.RandomNumberGenerator.Next(1, 10);
            await ctx.Channel.SendMessageAsync($"You little beggar! Fortunately people are kind and gave you {GoldAmount} gold");
            await _profileService.AddGold(ctx.Member!.Id, ctx.Guild.Id, GoldAmount);
        }


        [Command("equipment"), AddXP(10)]
        public async Task GetEquipment(CommandContext ctx, DiscordMember? member = null)
        {
            if (member == null)
                await DisplayEquipmentAsync(ctx, ctx.Member!.Id).ConfigureAwait(false);
            else
                await DisplayEquipmentAsync(ctx, member.Id).ConfigureAwait(false);
        }

        private async Task DisplayEquipmentAsync(CommandContext ctx, ulong memberID)
        {
            Profile profile = await _profileService.GetOrCreateProfileAsync(memberID, ctx.Guild.Id).ConfigureAwait(false);

            DiscordMember member = ctx.Guild.Members[profile.DiscordID];


            var profileEmbed = new DiscordEmbedBuilder
            {
                Title = $"{ctx.Guild.Members[profile.DiscordID].DisplayName}'s equipment",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = member.AvatarUrl },
                Color = DiscordColor.Blue
            };

            foreach (var item in profile.Equipment)
                profileEmbed.AddField(item.Type.ToString(), item.Name, true);

            await ctx.Channel.SendMessageAsync(embed: profileEmbed).ConfigureAwait(false);
        }

    }
}
