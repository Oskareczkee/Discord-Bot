//"mamonPhotoURL": "URLhttps://cdn.discordapp.com/attachments/736704747418943489/951933063955353730/unknown.png"

namespace Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Startup.ConfigureServices(builder.Services);
            var app = builder.Build();

            app.Run();

            Task.Delay(-1);
        }
    }
}
