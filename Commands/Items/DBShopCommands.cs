using Bot.Handlers.Dialogue;
using Bot.Handlers.Dialogue.Steps;
using Core.Services.Items;
using Core.Services.Profiles;
using DB.Models.Items.Enums;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bot.Commands.Items
{
    public partial class DBShopCommands : BaseCommandModule
    {
        //the database
        private readonly IItemService _itemService;
        private readonly IItemShopService _itemShopService;
        private readonly IProfileService _profileService;

        public DBShopCommands(IItemService ItemService, IItemShopService itemShopService, IProfileService profileService)
        {
            _itemService = ItemService;
            _itemShopService = itemShopService;
            _profileService = profileService;
        }

        [Command("itemlist")]
        [Description("Shows you list of all items")]
        public async Task ItemList(CommandContext ctx)
        {
            var items = await _itemService.GetItemsListAsync(ctx.Guild.Id).ConfigureAwait(false);

            var embed = new DiscordEmbedBuilder
            {
                Title = "All available items",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = Bot.Configuration.mamonPhotoURL },
                Color = DiscordColor.HotPink
            };

            var itemNames = new List<string>();

            //if there are no items implemented yet
            if (items.Count == 0)
            {
                embed.AddField("There are no items yet!", "Sorry");
                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                return;
            }

            foreach (var item in items)
                itemNames.Add(item.Name);

            embed.AddField("Items: ", string.Join(", ", itemNames));

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
        }


        [Command("getitem")]
        [Description("Gets specified item and shows its stats")]
        public async Task GetItem(CommandContext ctx)
        {
            var itemStep = new TextStep("What item are you looking for?", null);

            string itemName = string.Empty;

            itemStep.OnValidResult += (result) => itemName = result;

            var userChannel = await ctx.Member!.CreateDmChannelAsync().ConfigureAwait(false);

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                itemStep
                );

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded)
            {
                await ctx.Channel.SendMessageAsync("Something went wrong with the getitem dialogue, Sorry Pal!");
                return;
            }

            var item = await _itemService.GetItemByNameAsync(itemName, ctx.Guild.Id).ConfigureAwait(false);

            if (item == null)
            {
                await ctx.Channel.SendMessageAsync($"item: \"{itemName}\" does not exist ");
                return;
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = item.Name,
                Description = item.Description,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = Bot.Configuration.mamonPhotoURL },
                Color = DiscordColor.PhthaloBlue
            };

            embed.AddField("Type: ", item.Type.ToString(), true);
            embed.AddField("Price: ", item.Price.ToString(), true);
            embed.AddField("Armor: ", item.Armor.ToString(), true);
            embed.AddField("Strength: ", item.Strength.ToString(), true);
            embed.AddField("Agility: ", item.Agility.ToString(), true);
            embed.AddField("Intelligence: ", item.Intelligence.ToString(), true);
            embed.AddField("Endurance: ", item.Endurance.ToString(), true);
            embed.AddField("Luck: ", item.Luck.ToString(), true);

            if (item.Type == ItemType.Weapon)
            {
                embed.AddField("Min Damage: ", item.MinDamage.ToString());
                embed.AddField("Max Damage: ", item.MaxDamage.ToString());
            }

            await ctx.Channel.SendMessageAsync(embed: embed);
            return;
        }
    }
}
