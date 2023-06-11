using Bot.Commands;
using Bot.Commands.Combat;
using Bot.Commands.Helpers;
using Bot.Commands.Items;
using Bot.Commands.ProfileManagment;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
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

        public DiscordClient Client { get; private set; }// private set means we can set this Client only in this class
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public DiscordActivity Activity { get; private set; }

        //use it only when necessary or in attributes, othwerwise use constructor injection
        public static IServiceProvider Services { get; private set; } = null!;

        public static BotConfig Configuration { get; private set; } = null!;

        public Bot(IServiceProvider services)
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

            Client = new DiscordClient(Config);
            Client.Ready += OnClientReady;
            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            var CommandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.CommandPrefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                Services = services
            };

            Services = services;

            Commands = Client.UseCommandsNext(CommandsConfig);
            //command registration
            registerCommands();

            Client.ConnectAsync(Activity);
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
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
#if DATABASE_CLEAR
            Commands.RegisterCommands<ClearDatabase>();
#endif

            return Task.CompletedTask;
        }


    }
}
