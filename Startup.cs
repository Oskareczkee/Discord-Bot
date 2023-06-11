using Core.Services.Combat;
using Core.Services.Items;
using Core.Services.Mobs;
using Core.Services.Profiles;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Bot
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Context;Trusted_Connection=True;MultipleActiveResultSets=true");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IExperienceService, ExperienceService>();
            services.AddScoped<ICombatService, CombatService>();
            services.AddScoped<IMobService, MobService>();
            services.AddScoped<IItemShopService, ItemShopService>();
#if DATABASE_CLEAR
            services.AddScoped<IDataBaseClearService, DataBaseClearService>();
#endif


            var serviceProvider = services.BuildServiceProvider();

            var bot = new Bot(serviceProvider);
            services.AddSingleton(bot);
        }

        public void Configure(IApplicationBuilder builder, IWebHostEnvironment env)
        {

        }
    }
}
