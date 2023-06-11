using Microsoft.EntityFrameworkCore;
using Core.Math;
using DB.Models.Mobs;
using DB;

namespace Core.Services.Mobs
{
    public interface IMobService
    {
        Task CreateMob(Mob mob);
        Task<Mob> GetMobByName(string name);
        Task<Mob> GetRandomMob();

        //scales mob to specific level
        Mob ScaleMob(Mob mob, int level);
    }

    public class MobService : IMobService
    {
        private readonly DbContextOptions<Context> _options;
        public MobService(DbContextOptions<Context> options)
        {
            _options = options;
        }

        public async Task CreateMob(Mob mob)
        {
            using var _context = new Context(_options);

            await _context.Mobs.AddAsync(mob).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Mob> GetMobByName(string name)
        {
            using var _context = new Context(_options);
            return await _context.Mobs.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()).ConfigureAwait(false);
        }

        public async Task<Mob> GetRandomMob()
        {
            using var _context = new Context(_options);
            int randomIndex = BotMath.RandomNumberGenerator.Next(1, _context.Mobs.Count());

            return await _context.Mobs.Where(x => x.ID == randomIndex).FirstOrDefaultAsync();
        }

        public Mob ScaleMob(Mob mob, int level)
        {
            return BotMath.ScaleMob(mob, level);
        }
    }
}
