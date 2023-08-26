using DSharpPlus.Lavalink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Music
{
    public interface IMusicService
    {
        Task<LavalinkLoadResult> LoadTracksAsync(string SearchString);
        Task<LavalinkLoadResult> LoadTracksAsync(Uri uri);
    }
}
