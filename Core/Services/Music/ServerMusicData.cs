using DB.Models.Servers;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Music
{
    public class ServerMusicData
    {
        ServerMusicSettings Settings;
        public bool IsPlaying { get; set; }
        public MusicData? ActualTrack { get; set; } = default;
        public DiscordChannel? ConnectedChannel { get; set; }

        private LavalinkGuildConnection? Player { get; set; } = null!;
        private DiscordGuild Guild { get; set; }
        private LavalinkNodeConnection Lavalink { get; set; }

        public ServerMusicData(DiscordGuild guild, ServerMusicSettings settings, LavalinkNodeConnection lavalink)
        {
            Guild = guild;
            Settings = settings;
            Lavalink = lavalink;
        }

        public async Task PlayAsync(MusicData track)
        {
            if (Player == null || !Player.IsConnected)
                return;

            IsPlaying = true;
            ActualTrack = track;
            await Player.PlayAsync(track.Track);
        }

        public async Task StopAsync()
        {
            if (Player == null || !Player.IsConnected)
                return;

            ActualTrack = null;
            await Player.StopAsync();
        }

        public async Task PauseAsync()
        {
            if (Player == null || !Player.IsConnected)
                return;

            IsPlaying = false;
            await Player.PauseAsync();
        }

        public async Task ResumeAsync()
        {
            if (Player == null || !Player.IsConnected)
                return;

            IsPlaying = true;
            await Player.ResumeAsync();
        }

        public async Task SetVolumeAsync(int volume)
        {
            if (Player == null || !Player.IsConnected)
                return;

            await Player.SetVolumeAsync(volume);
            Settings.Volume = volume;
        }

        public async Task CreatePlayerAsync(DiscordChannel channel)
        {
            if (Player != null && Player.IsConnected)//player exists
                return;

            Player = await Lavalink.ConnectAsync(channel);
            ConnectedChannel = channel;
            if (Settings.Volume != 100)
                await Player.SetVolumeAsync(Settings.Volume);

            Player.PlaybackFinished += PlaybackFinished;
        }

        public async Task DestroyPlayerAsync()
        {
            if (Player == null)
                return;

            if (Player.IsConnected)
                await Player.DisconnectAsync();

            Player = null;
        }

        private async Task PlaybackFinished(LavalinkGuildConnection connection, TrackFinishEventArgs e)
        {
            return;
        }
    }
}
