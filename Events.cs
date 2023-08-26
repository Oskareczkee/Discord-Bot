using Core.Services.Servers;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using Microsoft.AspNetCore.Components.Routing;

namespace Bot
{
    public static class Events
    {
        public static async Task OnGuildCreated(object sender, GuildCreateEventArgs e)
        {
            var ServerService = Bot.Services.GetService<IServerService>();
            _ = await ServerService!.GetOrCreateServerAsync(e.Guild.Id).ConfigureAwait(false);

            var channel = e.Guild.GetDefaultChannel();
            await channel.SendMessageAsync("Siema, se dolaczylem na serwerek a co?").ConfigureAwait(false);

            Console.WriteLine($"Bot has been succesfully added to the guild {e.Guild.Id}");
        }

        public static async Task OnGuildDeleted(object sender, GuildDeleteEventArgs e)
        {
            var ServerService = Bot.Services.GetService<IServerService>();

            await ServerService!.DeleteServerAsync(e.Guild.Id);
            Console.WriteLine($"Bot has been succesfully deleted from the guild {e.Guild.Id}");
        }
    }
}
