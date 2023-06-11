using DSharpPlus;
using DSharpPlus.Entities;

namespace Bot.Handlers.Dialogue.Steps
{
    public class VoidStep : DialogueStepBase
    {

        public VoidStep(string content) : base(content)
        { }

        public override IDialogueStep NextStep => null;

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var cancelEmoji = DiscordEmoji.FromName(client, ":x:");

            var embedBuilder = new DiscordEmbedBuilder
            {
                Description = $"{user.Mention}, {_content}",
                Color = DiscordColor.HotPink
            };

            await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

            return false;
        }
    }
}
