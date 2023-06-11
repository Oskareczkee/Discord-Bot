using Bot.Handlers.Dialogue;
using Bot.Handlers.Dialogue.Steps;
using Core.Services.Items;
using DB.Models.Items;
using DB.Models.Profiles;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using static Core.Math.BotMath;

namespace Bot.Commands.Items
{
    public partial class DBShopCommands : BaseCommandModule
    {
        [Command("shop")]
        public async Task Shop(CommandContext ctx)
        {
            Profile profile = await _profileService.GetOrCreateProfileAsync(ctx.Member.Id, ctx.Guild.Id).ConfigureAwait(false);

            var shopItems = await _itemShopService.GetOrCreatePlayerShop(profile.DiscordID, profile.GuildID);
            int chosenItem = 0;

            var shopEmbed = new DiscordEmbedBuilder
            {
                Title = $"{ctx.Member.DisplayName}'s Crazy Item Shop",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = Bot.Configuration.mamonPhotoURL },
                Color = DiscordColor.Green,
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"Type 0 to leave\nType {shopItems.Count + 1} to reroll shop for {ShopRerollCost(profile.Level)} gold" }
            };

            shopEmbed.AddField($"{ctx.Member.DisplayName}'s gold:", profile.Gold.ToString());

            for (int x = 1; x <= shopItems.Count; x++)
                shopEmbed.AddField(x.ToString() + "." + shopItems[x - 1].Name,
                    @$"Type: {shopItems[x - 1].Type}
                    {shopItems[x - 1].StatsString()}
                    Price: {shopItems[x - 1].Price}
                    Modifiers
                    {shopItems[x - 1].ModifiersString()}",
                    true);

            await ctx.Channel.SendMessageAsync(embed: shopEmbed).ConfigureAwait(false);

            var itemStep = new IntStep(@"Please Choose item", null, 0, shopItems.Count + 1);

            itemStep.OnValidResult += (result) => chosenItem = result;

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                itemStep
                );

            bool succeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeded || chosenItem == 0)
                return;

            //reroll case
            if (chosenItem == 13)
            {
                bool enoughGold = await _itemShopService.RerollItemShop(profile.DiscordID, profile.GuildID, ShopRerollCost(profile.Level)).ConfigureAwait(false);

                if (!enoughGold)
                {
                    await ctx.Channel.SendMessageAsync($"You do not have enough gold to reroll!").ConfigureAwait(false);
                    return;
                }

                await ctx.Channel.SendMessageAsync($"Shop has been succesfully rerolled!").ConfigureAwait(false);

                //this recurrence call stinks badly
                await Shop(ctx).ConfigureAwait(false);
                return;
            }

            IItem item = shopItems[chosenItem - 1];

            //await _itemService.AddItemAsync(ctx.Member.Id, ctx.Guild.Id, item);
            ItemPurchaseState purchaseState = await _itemService.PurchaseItemAsync(ctx.Member.Id, ctx.Guild.Id, item).ConfigureAwait(false);

            switch (purchaseState)
            {
                case ItemPurchaseState.ItemNotFound:
                    await ctx.Channel.SendMessageAsync($"Item {item.Name} has not been found in shop!").ConfigureAwait(false);
                    break;
                case ItemPurchaseState.NotEnoughGold:
                    await ctx.Channel.SendMessageAsync($"You don't have enough gold to buy {item.Name}!").ConfigureAwait(false);
                    break;
                case ItemPurchaseState.EverythingWentGood:
                    {
                        await ctx.Channel.SendMessageAsync($"item {item.Name} has been successfully bought!").ConfigureAwait(false);
                        bool itemChangedProperly = await _itemShopService.ChangeShopItem(profile.DiscordID, profile.GuildID, chosenItem - 1, (await _itemShopService.GetShopItems(1, profile.Level)).FirstOrDefault());
                        if (!itemChangedProperly)
                            await ctx.Channel.SendMessageAsync($"But... Somehow next item in the shop couldn't be changed").ConfigureAwait(false);
                        break;
                    }
                default:
                    await ctx.Channel.SendMessageAsync("Something wrong happened in BuyItem function, please contact owner of this bot").ConfigureAwait(false);
                    break;
            }
        }
    }
}
