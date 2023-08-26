/*
 * if you want to clearing database be available
 * click on Core->Properties->Build->Conditional compilation symbols and add here DATABASE_CLEAR
 * do the same on Bot project
 * please do not use #define DATABASE_CLEAR here
 */

#if DATABASE_CLEAR

namespace Core.Services
{
    public interface IDataBaseClearService
    {
        Task ClearDataBase();
        Task ClearProfiles();
    }

    public class DataBaseClearService : IDataBaseClearService
    {
        private readonly DbContextOptions<Context> _options;

        public DataBaseClearService(DbContextOptions<Context> options)
        {
            _options = options;
        }

        public async Task ClearDataBase()
        {
            using var context = new Context(_options);

            context.Database.EnsureDeleted();
            //if database cannot get cleared put comment on line below and run the command
            await context.Database.MigrateAsync();
        }

        public async Task ClearProfiles()
        {
            using var context = new Context(_options);
            context.Profiles.RemoveRange(context.Profiles);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
#endif