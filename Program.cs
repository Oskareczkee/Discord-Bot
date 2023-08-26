//"mamonPhotoURL": "URLhttps://cdn.discordapp.com/attachments/736704747418943489/951933063955353730/unknown.png"

using Emzi0767.Utilities;

namespace Bot
{
    class Program
    {
        public static void Main(string[] args)
        {
            var async = new AsyncExecutor();
            async.Execute(MainAsync(args));
        }

        private static async Task MainAsync(string[] args)
        {
            await new Bot().StartAsync();
            await Task.Delay(-1);
        }
    }
}
