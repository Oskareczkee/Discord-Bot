﻿using Core.Services.Items;
using Core.Services.Profiles;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using static Bot.Bot;

namespace Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequireItem : CheckBaseAttribute
    {
        public string itemName { get; private set; }
        private readonly IItemService ItemService;
        private readonly IProfileService ProfileService;
        public RequireItem(string name)
        {
            itemName = name;
            ItemService = Services.GetService(typeof(IItemService)) as IItemService;
            ProfileService = Services.GetService(typeof(IProfileService)) as IProfileService;
        }

        public async override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            var Item = await ItemService.GetItemByNameAsync(itemName, ctx.Guild.Id);
            if (Item == null)
            {
                await ctx.Channel.SendMessageAsync($"Attribute: RequireItem - Item {itemName} has not been found in items database");
                return false;
            }

            bool succeeded = await ProfileService.UseItemAsync(ctx.Member.Id, ctx.Guild.Id, Item);

            if (succeeded)
                return true;

            await ctx.Channel.SendMessageAsync($"You do not have item {itemName} in your inventory");
            return false;

        }
    }
}
