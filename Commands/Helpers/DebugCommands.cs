using Core.Services.Items;
using Core.Services.Profiles;
using DB.Models.Items;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bot.Commands.Helpers
{
    public class DebugCommands : BaseCommandModule
    {
        private readonly IItemService _itemService;
        private readonly IProfileService _profileService;
        private readonly IExperienceService _expService;

        public DebugCommands(IItemService ItemService, IProfileService profileService, IExperienceService expService)
        {
            _itemService = ItemService;
            _profileService = profileService;
            _expService = expService;
        }

        //Debug command to add items and test them for free
        [RequireOwner]
        [Command("spawnitem")]
        [Description("Debug command for adding items")]
        public async Task SpawnItem(CommandContext ctx, string itemName, DiscordMember mention = null)
        {
            IItem item = await _itemService.GetItemByName(itemName);

            if (item == null)
            {
                await ctx.Channel.SendMessageAsync($"SpawnItem - item {itemName} has not been found in database").ConfigureAwait(false);
                return;
            }

            if (mention == null)
                await _itemService.AddItemAsync(ctx.Member.Id, ctx.Guild.Id, item);
            else
                await _itemService.AddItemAsync(mention.Id, mention.Guild.Id, item);
            await ctx.Channel.SendMessageAsync($"Item {itemName} has been successfully added to your inventory");
        }

        [RequireOwner]
        [Command("addgold")]
        [Description("Debug command for adding gold")]
        public async Task AddGold(CommandContext ctx, int amount, DiscordMember mention = null)
        {
            if (mention == null)
            {
                await _profileService.AddGold(ctx.Member.Id, ctx.Guild.Id, amount).ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync($"{amount} gold has been donated to {ctx.Member.DisplayName}").ConfigureAwait(false);
            }
            else
            {
                await _profileService.AddGold(mention.Id, mention.Guild.Id, amount).ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync($"{amount} gold has been donated to {mention.DisplayName}").ConfigureAwait(false);
            }
        }

        [RequireOwner]
        [Command("addexp")]
        [Description("Debug command for adding experience")]
        public async Task AddExp(CommandContext ctx, int amount, DiscordMember mention = null)
        {
            if (mention == null)
            {
                await _expService.GrantXPAsync(ctx.Member.Id, ctx.Guild.Id, amount).ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync($"{amount} experience points has been given to {ctx.Member.DisplayName}").ConfigureAwait(false);
            }
            else
            {
                await _expService.GrantXPAsync(mention.Id, mention.Guild.Id, amount).ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync($"{amount} experience points has been given to {mention.DisplayName}").ConfigureAwait(false);
            }
        }
    }
}
