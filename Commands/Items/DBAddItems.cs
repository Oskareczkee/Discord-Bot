using Bot.Handlers.Dialogue;
using Bot.Handlers.Dialogue.Steps;
using DB.Models.Items;
using DB.Models.Items.Enums;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bot.Commands.Items
{
    public partial class DBShopCommands : BaseCommandModule
    {
        [Command("additem")]
        //[RequireOwner]

        //epic items are added to special db set called epic items
        public async Task AddItem(CommandContext ctx, bool isEpicItem = false)
        {
            ItemType type = ItemType.None;
            string itemDescription = string.Empty;
            string itemName = string.Empty;
            int itemPrice = 0;


            var itemPriceStep = new IntStep("Enter the item price", null);
            var itemDescriptionStep = new TextStep("Enter the item description", itemPriceStep);
            var itemNameStep = new TextStep("Enter the item name", itemDescriptionStep);

            var itemTypeStep = new IntStep(
                @"Please Enter the item type:
                Helmet = 1,
                Chestplate =2,
                Gloves =3,
                Shoes =4,
                Weapon=5,
                Ring=6,
                Belt=7,
                Necklace=8,
                Extra=9,
                Potion=10,
                Miscellaneous=11",
                itemNameStep, 1, 11);

            itemTypeStep.OnValidResult += (result) => type = (ItemType)result;
            itemNameStep.OnValidResult += (result) => itemName = result;
            itemDescriptionStep.OnValidResult += (result) => itemDescription = result;
            itemPriceStep.OnValidResult += (result) => itemPrice = result;

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                itemTypeStep
                );

            bool succeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            //checks whether given item exists

            if (!succeded)
            {
                await ctx.Channel.SendMessageAsync("Something went wrong with the additem dialogue, Sorry Pal!");
                return;
            }

            IItem itemCheck = await _itemService.GetItemByName(itemName).ConfigureAwait(false);
            if (itemCheck != null)
            {
                await ctx.Channel.SendMessageAsync($"IItem with name ${itemName} already exists in the database");
                return;
            }

            IItem? item = null;

            if (type == ItemType.Weapon)
                item = await AddWeapon(ctx, itemName, itemDescription, itemPrice).ConfigureAwait(false);
            else if (type == ItemType.Potion)
                item = await AddPotion(ctx, itemName, itemDescription, itemPrice);
            else if (type == ItemType.Miscellaneous)
                item = AddMiscellaneous(itemName, itemDescription, itemPrice);
            else
                item = await AddItem(ctx, itemName, itemDescription, itemPrice, type).ConfigureAwait(false);

            if (item == null)
                return;

            await _itemService.CreateNewItemAsync(item, isEpicItem);
            await ctx.Channel.SendMessageAsync($"IItem {item.Name} has been successfully added!").ConfigureAwait(false);
        }

        [Command("addepicitem")]
        public async Task AddEpicItem(CommandContext ctx)
        {
            await AddItem(ctx, true).ConfigureAwait(false);
        }

        private async Task<IItem?> AddItem(CommandContext ctx, string name, string description, int price, ItemType type)
        {
            ItemBase item = new()
            {
                Name = name,
                Description = description,
                Price = price,
                Type = type
            };

            string statsString = string.Empty;

            var itemStatsStep = new TextStep(@"Enter the item stats:
                                               Strength Agility Intelligence Endurance Luck Armor
                                               Example: 0 0 5 5 5 10"
                                               , null);

            itemStatsStep.OnValidResult += (result) => statsString = result;

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                itemStatsStep
                );

            if (!await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false))
                return null;


            List<string> statsSplit = statsString.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            //6 for normal item
            if (statsSplit.Count != 6)
            {
                await ctx.Channel.SendMessageAsync("Additem: wrong stats string (splitted string is too long or too short)").ConfigureAwait(false);
                return null;
            }

            List<int> StatsInt = new();

            foreach (string stat in statsSplit)
            {
                int value = 0;
                if (int.TryParse(stat, out value))
                    StatsInt.Add(value);
                else
                {
                    await ctx.Channel.SendMessageAsync("Additem: stats could not be parsed to an integer type, please try to add item once more").ConfigureAwait(false);
                    return null;
                }
            }

            item.Strength = StatsInt[0];
            item.Agility = StatsInt[1];
            item.Intelligence = StatsInt[2];
            item.Endurance = StatsInt[3];
            item.Luck = StatsInt[4];
            item.Armor = StatsInt[5];

            return item;
        }

        private async Task<IItem?> AddWeapon(CommandContext ctx, string name, string description, int price)
        {
            ItemBase item = new()
            {
                Name = name,
                Description = description,
                Price = price,
                Type = ItemType.Weapon
            };
            string statsString = string.Empty;
            string weaponStats = string.Empty;

            var weaponDamageStep = new TextStep(@"Enter weapon damage:
                                                 min damage, max damage
                                                 Example: 5 10"
                                                 , null);

            var itemStatsStep = new TextStep(@"Enter the weapon stats:
                                               Strength Agility Intelligence Endurance Luck
                                               Example: 0 0 5 5 5"
                                               , weaponDamageStep);


            weaponDamageStep.OnValidResult += (result) => weaponStats = result;
            itemStatsStep.OnValidResult += (result) => statsString = result;

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                itemStatsStep
                );

            if (!await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false))
                return null;


            List<string> statsSplit = statsString.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            statsSplit.AddRange(weaponStats.Split(" ", StringSplitOptions.RemoveEmptyEntries));

            if (statsSplit.Count != 7)
            {
                await ctx.Channel.SendMessageAsync("AddWeapon: wrong stats string (splitted string is too long or too short). Please check your weapon stats and weapon damage").ConfigureAwait(false);
                return null;
            }

            List<int> StatsInt = new();

            foreach (string stat in statsSplit)
            {
                int value = 0;
                if (int.TryParse(stat, out value))
                    StatsInt.Add(value);
                else
                {
                    await ctx.Channel.SendMessageAsync("AddWeapon: stats could not be parsed to an integer type, please try to add item once more").ConfigureAwait(false);
                    return null;
                }
            }

            item.Strength = StatsInt[0];
            item.Agility = StatsInt[1];
            item.Intelligence = StatsInt[2];
            item.Endurance = StatsInt[3];
            item.Luck = StatsInt[4];

            item.MinDamage = StatsInt[5];
            item.MaxDamage = StatsInt[6];

            return item;
        }

        private IItem AddMiscellaneous(string name, string description, int price)
        {
            return new ItemBase { Name = name, Description = description, Price = price, Type = ItemType.Miscellaneous };
        }

        private async Task<IItem?> AddPotion(CommandContext ctx, string name, string description, int price)
        {
            ItemBase item = new()
            {
                Name = name,
                Description = description,
                Price = price,
                Type = ItemType.Potion
            };

            string potionModifiers = string.Empty;

            var allModifiers = Enum.GetNames(typeof(Modifiers)).ToList();

            string modifiersString = string.Empty;

            for (int x = 1; x < allModifiers.Count; x++)
                modifiersString += x + ". " + allModifiers[x] + "\n";

            var potionModifiersStep = new TextStep($@"Enter potion modifiers:
                                                 {modifiersString}
                                                 Example: 1 10 2 -20 3 30
                                                 (10% bonus to Melee Damage, -20% to Magic Damage, 30% to Magic Attack Chance)"
                                     , null);

            potionModifiersStep.OnValidResult += (result) => potionModifiers = result;

            var inputDialogueHandler = new DialogueHandler(
                                        ctx.Client,
                                        ctx.Channel,
                                        ctx.User,
                                        potionModifiersStep
                                        );

            if (!await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false))
                return null;

            if (!Modifier.CheckModifiersStringValidity(potionModifiers))
            {
                await ctx.Channel.SendMessageAsync("AddPotion: Potion modifiers are not valid, please check your potion modifiers").ConfigureAwait(false);
                return null;
            }

            item.Modifiers = potionModifiers;

            return item;
        }
    }
}
