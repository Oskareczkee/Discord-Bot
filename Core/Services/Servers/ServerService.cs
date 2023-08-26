using DB;
using DB.Models.Servers;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Servers
{
    public class ServerService : IServerService
    {
        private readonly DbContextOptions<Context> _options;
        public ServerService(DbContextOptions<Context> options) => _options = options;

        public async Task DeleteServerAsync(ulong guildID)
        {
            using var context = new Context(_options);
            Server Server = new Server { GuildID = guildID };
            context.Attach(Server);
            context.Servers.Remove(Server);

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Server> GetOrCreateServerAsync(ulong guildID)
        {
            using var context = new Context(_options);

            Server? Server = await context.Servers.Where(s => s.GuildID == guildID).
                    Include(s => s.Profiles).
                    Include(s => s.Mobs).
                    Include(s => s.Items).
                    FirstOrDefaultAsync().ConfigureAwait(false);

            if (Server != null)
                return Server;

            Server = new Server {GuildID = guildID };
            await context.Servers.AddAsync(Server);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return Server;
        }


    }
}
