using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using System.Collections.Concurrent;

namespace Core.Services.Music
{
    public class MusicService
    {
        private DiscordClient Discord { get; }
        private LavalinkNodeConnection Lavalink { get; set; } = null!;

        private ConcurrentDictionary<ulong, ServerMusicData> MusicData { get; } = new ConcurrentDictionary<ulong, ServerMusicData>();

        public MusicService(DiscordClient client)
        {
            Discord = client;
            Discord.Ready += Client_Ready;
        }

        private async Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            var endpoint = new ConnectionEndpoint
            {
                Hostname = "lavalink.ordinaryender.my.eu.org",
                Port = 443,
                Secured = true
            };

            var lavalinkConfig = new LavalinkConfiguration
            {
                Password = "ordinarylavalink",
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };

            var lava = sender.GetLavalink();

            Lavalink = await lava.ConnectAsync(lavalinkConfig);
        }

        public async Task<LavalinkLoadResult> LoadTracksAsync(string SearchString) => await Lavalink.Rest.GetTracksAsync(SearchString);
        public async Task<LavalinkLoadResult> LoadTracksAsync(Uri uri) => await Lavalink.Rest.GetTracksAsync(uri);
        public bool LavalinkConnected()
        {
            if (Lavalink == null)
                return false;
            return Lavalink.IsConnected;
        }
        public async Task<ServerMusicData> GetOrCreateServerMusic(DiscordGuild guild)
        {
            if (MusicData.TryGetValue(guild.Id, out var smd))
                return smd;

            smd = MusicData.AddOrUpdate(guild.Id, new ServerMusicData(guild,
                new DB.Models.Servers.ServerMusicSettings { GuildID = guild.Id, Volume = 100 }, Lavalink), (key, value)=> value);

            return smd;
        }
    }
}
