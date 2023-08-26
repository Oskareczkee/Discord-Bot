using DB.Models.Items;
using DB.Models.Mobs;
using DB.Models.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Servers
{
    public interface IServerService
    {
        Task<Server> GetOrCreateServerAsync(ulong guildID);
        Task DeleteServerAsync(ulong guildID);
    }
}
