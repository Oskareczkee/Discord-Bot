using Core.Services.Music;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;

namespace Bot.Commands
{
    public class MusicCommands : BaseCommandModule
    {
        private readonly MusicService MusicService;
        private ServerMusicData? ServerMusic = null!;

        public MusicCommands(MusicService service) => MusicService = service;

        private async Task<bool> CanExecute(CommandContext ctx)
        {
            if(!MusicService.LavalinkConnected())
            {
                await ctx.RespondAsync("Lavalink connection is not established");
                return false;
            }

            var voice = ctx.Member?.VoiceState;
            var channel = voice?.Channel;

            if(channel==null || channel.Type!= DSharpPlus.ChannelType.Voice)
            {
                await ctx.RespondAsync("You have to be connected to the voice channel");
                return false;
            }

            ServerMusic = await MusicService.GetOrCreateServerMusic(ctx.Guild);

            if (ServerMusic.ConnectedChannel != channel && ServerMusic.ConnectedChannel!=null)
            {
                await ctx.RespondAsync("You have to be connected to the same voice channel");
                return false;
            }

            return true;
        }

        [Command("join")]
        public async Task Join(CommandContext ctx)
        {
            if (!await CanExecute(ctx))
                return;
            await ServerMusic.CreatePlayerAsync(ctx.Member?.VoiceState.Channel);
            await ctx.RespondAsync($"Joining {ctx.Member?.VoiceState.Channel.Name}");
        }

        [Command("leave")]
        public async Task Leave(CommandContext ctx)
        {
            if (!await CanExecute(ctx))
                return;

            if (ServerMusic.ConnectedChannel == null)
                await ctx.RespondAsync("Not connected to any channel");

            await ServerMusic.DestroyPlayerAsync();//even if not connected, destroy it anyway XD
        }

        [Command("play"), Description("Plays music from supplied url or searches for specified keywords"), Aliases("p"), Priority(1)]
        public async Task Play(CommandContext ctx, Uri uri)
        {
            if (!await CanExecute(ctx))
                return;

            var loadResult = await MusicService.LoadTracksAsync(uri);

            if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed
                || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                await ctx.RespondAsync("Link search failed, or is not a proper video link");
                return;
            }

            foreach (var Track in loadResult.Tracks)
                Console.WriteLine($"Track: {Track.Title} Link: {Track.Uri} Author: {Track.Author}");

            var track = loadResult.Tracks.First();
            var channel = ctx.Member.VoiceState.Channel;
            await ServerMusic.CreatePlayerAsync(channel);
            await ServerMusic.PlayAsync(new MusicData(track, ctx.Member));

            await ctx.RespondAsync($"Now playing {track.Title}");

        }

        [Command("play"), Priority(0)]
        public async Task Play(CommandContext ctx, params string[] search)
        {
            if (!await CanExecute(ctx))
                return;

            string searchStr = string.Join(' ', search);
            var loadResult = await MusicService.LoadTracksAsync(searchStr);

            if(loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed
                || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                await ctx.RespondAsync($"Track search failed for {searchStr}");
                return;
            }

            foreach (var Track in loadResult.Tracks)
                Console.WriteLine($"Track: {Track.Title} Link: {Track.Uri} Author: {Track.Author}");

            var track = loadResult.Tracks.First();
            var channel = ctx.Member.VoiceState.Channel;
            await ServerMusic.CreatePlayerAsync(channel);
            await ServerMusic.PlayAsync(new MusicData(track, ctx.Member));

            await ctx.RespondAsync($"Now playing {track.Title}");

        }
    }
}
