using Bot.Handlers.Dialogue;
using Bot.Handlers.Dialogue.Steps;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bot.Commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("dialogue")]
        [Description("Dialogue Test")]

        public async Task Dialogue(CommandContext ctx)
        {
            string input = string.Empty;
            int value = 0;

            var inputStep = new TextStep("Enter something interesting!", null, 5, 50);
            var intStep = new IntStep("Enter funny value pls", null, maxValue: 100);

            inputStep.OnValidResult = (result) =>
            {
                input = result;

                if (result.ToLower() == "something interesting")
                    inputStep.SetNextStep(intStep);
            };

            intStep.OnValidResult = (result) => value = result;

            var userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

            var inputDialogueHandler = new DialogueHandler(ctx.Client, userChannel, ctx.User, inputStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded)
                return;

            await ctx.Channel.SendMessageAsync(input).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync(value.ToString()).ConfigureAwait(false);
        }

        [Command("emojidialogue")]
        [Description("Emoji dialogue test")]

        public async Task EmojiDialogue(CommandContext ctx)
        {
            var yesStep = new VoidStep("You chose yes !");
            var noStep = new VoidStep("You chose No !");

            var EmojiStep = new ReactionStep("Yes or No?", new Dictionary<DiscordEmoji, ReactionStepData>
            {
                {DiscordEmoji.FromName(ctx.Client, ":thumbsup:"), new ReactionStepData{ Content = "This mean yes", NextStep = yesStep} },
                {DiscordEmoji.FromName(ctx.Client, ":thumbsdown:"), new ReactionStepData{ Content = "This mean no", NextStep = noStep} },
            });

            var userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                userChannel,
                ctx.User,
                EmojiStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded) return;
        }
    }
}
