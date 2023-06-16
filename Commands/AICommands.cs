using Core.Services.AI;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bot.Commands
{
    public class AICommands : BaseCommandModule
    {
        private readonly IAIService _AIService;

        public AICommands(IAIService AIService)
        {
            _AIService = AIService;
        }

        [Command("ask")]
        [Description("ask anything you want")]
        public async Task Ask(CommandContext ctx, params string[] message)
        {
            string mess = String.Join(' ', message);
            var response = await _AIService.GenerateContent(mess).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(response).ConfigureAwait(false);

        }

    }
}
