using DSharpPlus.Entities;
using DSharpPlus.Lavalink;

namespace Core.Services.Music
{
    /// <summary>
    /// Class for storing music data and user who requested it
    /// </summary>
    public class MusicData
    {
        public LavalinkTrack Track { get; }
        public DiscordMember RequestedBy { get; }

        public MusicData(LavalinkTrack t, DiscordMember request)
        {
            Track = t;
            RequestedBy = request;
        }
    }
}
