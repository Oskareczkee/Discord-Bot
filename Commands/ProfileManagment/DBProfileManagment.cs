using Bot.Handlers.Dialogue;
using Bot.Handlers.Dialogue.Steps;
using Core.Math;
using DB.Models.Items;
using DB.Models.Items.Enums;
using DB.Models.Profiles;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bot.Commands.ProfileManagment
{
    public partial class DBProfileCommands : BaseCommandModule
    {

        [Command("addskill")]
        [Description("Allows to add skills to the profile")]
        public async Task AddSkill(CommandContext ctx)
        {
            ulong price = 0;

            Profile profile = await _profileService.GetOrCreateProfileAsync(ctx.Member!.Id, ctx.Guild.Id);
            var skillAmountStep = new IntStep("How much levels do you want to buy", null, 1, 100);

            var skillStep = new IntStep(
                $@"Which skill do you want to level up:
                Strength = 1 (Cost: {BotMath.SkillLevelUpCost(profile.Strength)})
                Agility = 2 (Cost: {BotMath.SkillLevelUpCost(profile.Agility)})
                Intelligence = 3 (Cost: {BotMath.SkillLevelUpCost(profile.Intelligence)})
                Endurance = 4 (Cost: {BotMath.SkillLevelUpCost(profile.Endurance)})
                Luck = 5 (Cost: {BotMath.SkillLevelUpCost(profile.Luck)})",
                skillAmountStep, 1, 5);

            SkillType skillType = SkillType.None;
            int skillAmount = 0;
            DiscordEmoji? confirmationEmoji = null;

            skillStep.OnValidResult += (result) => skillType = (SkillType)result;
            skillAmountStep.OnValidResult += (result) => skillAmount = result;


            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                skillStep
                );

            bool succeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);
            if (!succeded)
            {
                await ctx.Channel.SendMessageAsync("Something went wrong with AddSkill dialogue, Sorry").ConfigureAwait(false);
                return;
            }

            price = BotMath.calculatePrice(skillType, profile, skillAmount);

            var confirmationStep = new ReactionStep(
                $@"This will cost you {price} gold
                   You have: {profile.Gold} gold
                   React with {DiscordEmoji.FromName(ctx.Client, ":white_check_mark:")} to confirm or {DiscordEmoji.FromName(ctx.Client, ":x:")} to cancel operation",
                new System.Collections.Generic.Dictionary<DiscordEmoji, ReactionStepData>
            {
                {DiscordEmoji.FromName(ctx.Client, ":white_check_mark:"), new ReactionStepData{ Content = "", NextStep = null} },
             });

            confirmationStep.OnValidResult += (result) => confirmationEmoji = result;

            var confirmationDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                confirmationStep
                );

            await confirmationDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (confirmationEmoji == DiscordEmoji.FromName(ctx.Client, ":white_check_mark:"))
            {
                if (profile.Gold < price)
                {
                    await ctx.Channel.SendMessageAsync("You do not have enough gold").ConfigureAwait(false);
                    return;
                }

                await _profileService.LevelUpSkillAsync(profile.DiscordID, profile.GuildID ?? 0, skillType, skillAmount, price);
                await ctx.Channel.SendMessageAsync("Skill has been succesfully levelled up").ConfigureAwait(false);
            }

        }

        [Command("equip")]
        public async Task EqiupItem(CommandContext ctx)
        {
            Profile profile = await _profileService.GetOrCreateProfileAsync(ctx.Member!.Id, ctx.Guild.Id);

            ItemType itemType = ItemType.Miscellaneous;

            var itemTypeStep = new IntStep(
            @"What item do you want to change:
                Helmet = 1,
                Chestplate =2,
                Gloves =3,
                Shoes =4,
                Weapon=5,
                Ring=6,
                Belt=7,
                Necklace=8,
                Extra=9",
                null, 1, 9);

            itemTypeStep.OnValidResult += (result) => itemType = (ItemType)result;

            var typeDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                itemTypeStep
                );

            bool succeded = await typeDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeded)
                return;

            List<Item> availableItems = (from item in profile.Items where item.Type == itemType select item as Item).ToList();

            var itemEmbed = new DiscordEmbedBuilder
            {
                ImageUrl = Bot.Configuration.mamonPhotoURL,
                Title = "Inventory",
                Description = $"All items of type {itemType} in your inventory",
            };


            //gets all item names for embed and its statistics and puts it into a string
            List<string> itemNames = (from item in availableItems
                                      select item.Name + " {Armor:" + item.Armor
                                      + " S:" + item.Strength.ToString()
                                      + " A:" + item.Agility.ToString()
                                      + " I:" + item.Intelligence
                                      + " E:" + item.Endurance.ToString()
                                      + " L:" + item.Luck.ToString() + "}").ToList();
            itemNames.Add("Unequip");

            //add item to unequip
            availableItems.Add(new EquipmentItem(profile.ID, itemType));

            //adding counters for items (I could propably do this in query, but that seemed to be problematic, this is just simpler approach)
            //Insert operation on string might be ineffective 
            //Good way to do this would be propably do the strings with counters and then concat to them string from query
            for (int x = 1; x <= itemNames.Count; x++)
                itemNames[x - 1] = itemNames[x - 1].Insert(0, x.ToString() + ". ");

            itemEmbed.AddField("Items: ", string.Join("\n", itemNames));

            await ctx.Channel.SendMessageAsync(embed: itemEmbed);


            int choice = 0;
            var chooseItemStep = new IntStep("Please choose index of item you want to equip", null, 1, itemNames.Count);
            chooseItemStep.OnValidResult = (result) => choice = result;

            var chooseDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                chooseItemStep
                );

            succeded = await chooseDialogueHandler.ProcessDialogue().ConfigureAwait(false);
            if (!succeded)
                return;

            bool changed = await _profileService.EquipItemAsync(profile.DiscordID, profile.GuildID ?? 0, availableItems[choice - 1]);

            if (changed)
                if (availableItems[choice - 1].Name.Equals("None", StringComparison.OrdinalIgnoreCase))
                    await ctx.Channel.SendMessageAsync("Item has been succesfully unequipped").ConfigureAwait(false);
                else
                    await ctx.Channel.SendMessageAsync("Item has been succesfully equipped").ConfigureAwait(false);
            else
                await ctx.Channel.SendMessageAsync("You have no item equipped").ConfigureAwait(false);
        }


        [Command("unequip")]
        public async Task UneqiupItem(CommandContext ctx)
        {
            Profile profile = await _profileService.GetOrCreateProfileAsync(ctx.Member!.Id, ctx.Guild.Id);

            ItemType itemType = ItemType.Miscellaneous;

            var itemTypeStep = new IntStep(
            @"What item do you want to unequip:
                Helmet = 1,
                Chestplate =2,
                Gloves =3,
                Shoes =4,
                Weapon=5,
                Ring=6,
                Belt=7,
                Necklace=8,
                Extra=9",
                null, 1, 9);

            itemTypeStep.OnValidResult += (result) => itemType = (ItemType)result;

            var typeDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                itemTypeStep
                );

            bool succeded = await typeDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeded)
                return;

            bool changed = await _profileService.EquipItemAsync(profile.DiscordID, profile.GuildID ?? 0, new EquipmentItem(profile.ID, itemType));

            if (changed)
                await ctx.Channel.SendMessageAsync("Item has been succesfully unequipped").ConfigureAwait(false);
            else
                await ctx.Channel.SendMessageAsync("You have no item equipped").ConfigureAwait(false);
        }

        [Command("consumeitem")]
        public async Task UseItem(CommandContext ctx)
        {
            Profile profile = await _profileService.GetOrCreateProfileAsync(ctx.Member!.Id, ctx.Guild.Id);

            Item? chosenItem = null;

            List<Item> availableItems = (from item in profile.Items where item.Type == ItemType.Potion select item as Item).ToList();

            if (availableItems.Count == 0)
            {
                await ctx.Channel.SendMessageAsync("You have no consumables in your inventory :(").ConfigureAwait(false);
                return;
            }

            List<string> availableItemsString = new();

            for (int x = 0; x < availableItems.Count; x++)
            {
                string itemString = $"{x + 1}. {availableItems[x].Name}\n{availableItems[x].ModifiersString()}";
                availableItemsString.Add(itemString);
            }

            var itemEmbed = new DiscordEmbedBuilder
            {
                Title = "Inventory",
                Description = $"All consumables in your inventory",
            };

            itemEmbed.AddField("Consumables: ", string.Join("\n", availableItemsString));
            await ctx.Channel.SendMessageAsync(embed: itemEmbed).ConfigureAwait(false);

            var itemTypeStep = new IntStep(
            @"Which item do you want to use?",
                null, 1, availableItems.Count);

            itemTypeStep.OnValidResult += (result) => chosenItem = availableItems[result - 1];

            var typeDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                itemTypeStep
                );


            bool succeded = await typeDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeded)
                return;

            var playerEqConsumables = new DiscordEmbedBuilder
            {
                Title = "Your Consumables slots",
                Description = $"Consumables that you have on you",
            };

            for (int x = 9; x < 12; x++)
                playerEqConsumables.AddField($"{x - 8}. {profile.Equipment[x].Name}", $"{((Item)(profile.Equipment[x])).ModifiersString()}");

            int chosenSlot = 0;

            var itemSlotStep = new IntStep(
                               @"Choose consumable slot",
                               null, 1, 3);
            itemSlotStep.OnValidResult += (result) => chosenSlot = result;

            var slotDialogueHandler = new DialogueHandler(
                                        ctx.Client,
                                        ctx.Channel,
                                        ctx.User,
                                        itemSlotStep
                                        );

            await ctx.Channel.SendMessageAsync(embed: playerEqConsumables).ConfigureAwait(false);
            if (!await slotDialogueHandler.ProcessDialogue().ConfigureAwait(false))
                return;


            if (!await _profileService.UseConsumableAsync(ctx.Member.Id, ctx.Guild.Id, chosenItem, chosenSlot))
            {
                await ctx.Channel.SendMessageAsync($"UseConsumableAsync: Item could not be consumed");
                return;
            }

            await ctx.Channel.SendMessageAsync($"{chosenItem!.Name} has been successfully consumed!");
        }
    }
}
