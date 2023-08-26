using Bot.Commands;
using Bot.Commands.Combat;
using Bot.Commands.Helpers;
using Bot.Commands.Items;
using Bot.Commands.ProfileManagment;
using Core.Services.AI;
using Core.Services.Combat;
using Core.Services.Items;
using Core.Services.Mobs;
using Core.Services.Music;
using Core.Services.Profiles;
using Core.Services.Servers;
using DB;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Bot
{
    public class Bot
    {
        //this class grants easy access to bot config properties
        public class BotConfig
        {
            public string prefix { get; set; } = string.Empty;
            public string mamonPhotoURL { get; set; } = string.Empty;
            public ulong IDPawla { get; set; }
            public string MemeFolderRoot { get; set; } = string.Empty;
        }

        public DiscordClient Client { get; private set; } = null!;
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public DiscordActivity Activity { get; private set; }
        public LavalinkExtension Lavalink { get; private set; } = null!;
        //use it only when necessary or in attributes, othwerwise use constructor injection
        public static IServiceProvider Services { get; private set; } = null!;

        public static BotConfig Configuration { get; private set; } = null!;

        public Bot()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = sr.ReadToEnd();

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var Config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                Intents = DiscordIntents.AllUnprivileged
            };

            Configuration = new BotConfig
            {
                prefix = configJson.CommandPrefix,
                mamonPhotoURL = configJson.mamonPhotoURL,
                IDPawla = configJson.IDPawla,
                MemeFolderRoot = configJson.MemeFolderRoot
            };

            Activity = new DiscordActivity
            {
                Name = "Momenty Brata",
                ActivityType = ActivityType.Streaming,
                StreamUrl = "https://www.youtube.com/watch?v=bzZ4G3z5TMY",
            };

            //build services
            Services = new ServiceCollection()
                .AddDbContext<Context>(options =>
                            {
                                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Context;Trusted_Connection=True;MultipleActiveResultSets=true");
                                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                            })
            .AddScoped<IItemService, ItemService>()
            .AddScoped<IProfileService, ProfileService>()
            .AddScoped<IExperienceService, ExperienceService>()
            .AddScoped<ICombatService, CombatService>()
            .AddScoped<IMobService, MobService>()
            .AddScoped<IItemShopService, ItemShopService>()
            .AddScoped<IAIService, AIService>()
            .AddScoped<IServerService, ServerService>()
            .AddScoped<IConfiguration>(_ => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build())
            .AddSingleton(this)
            .AddSingleton<MusicService>(_ => new MusicService(Client))
            .BuildServiceProvider();

            Client = new DiscordClient(Config);
            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            var CommandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.CommandPrefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                Services = Services
            };

            Lavalink = Client.UseLavalink();

            Commands = Client.UseCommandsNext(CommandsConfig);
            //command registration
            registerCommands();

            //register events
            Client.GuildCreated += Events.OnGuildCreated;
            Client.GuildDeleted += Events.OnGuildDeleted;
        }

        public Task StartAsync()
        {
            return Client.ConnectAsync(Activity);
        }

        //put all commands registers here
        private Task registerCommands()
        {
            //basic commands
            Commands.RegisterCommands<TestCommands>();
            Commands.RegisterCommands<ManageCommands>();
            Commands.RegisterCommands<BasicCommands>();
            Commands.RegisterCommands<DebugCommands>();

            //shop commands
            Commands.RegisterCommands<DBShopCommands>();

            //profile commands
            Commands.RegisterCommands<DBProfileCommands>();

            //combat commands
            Commands.RegisterCommands<DBCombatCommands>();

            //mobs
            Commands.RegisterCommands<DBMobCommands>();

            //ai commands
            Commands.RegisterCommands<AICommands>();

            //music commands
            Commands.RegisterCommands<MusicCommands>();
#if DATABASE_CLEAR
            Commands.RegisterCommands<ClearDatabase>();
#endif

            return Task.CompletedTask;
        }


    }
}
