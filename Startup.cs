﻿using Core.Services.AI;
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
        static public void ConfigureServices(IServiceCollection services)
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
            services.AddScoped<IAIService, AIService>();
#if DATABASE_CLEAR
            services.AddScoped<IDataBaseClearService, DataBaseClearService>();
#endif


            var serviceProvider = services.BuildServiceProvider();

            var bot = new Bot(serviceProvider);
            services.AddSingleton(bot);
        }

        static public void ConfigureApp(WebApplication app)
        {
        }
    }
}
