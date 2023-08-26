using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using Web_UI.Models.Discord;

namespace Web_UI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IDiscordAPIService Api = null!;

        public AuthenticationController(IDiscordAPIService _Api) => Api = _Api;

        public ViewResult AuthFailed()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes= "Discord")]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            var User = await Api.GetDiscordUser();
            var Guilds = await Api.GetDiscordUserGuilds();

            Console.WriteLine($"ID: {User.id} Name: {User.username}");
            Console.WriteLine("Guilds: ");
            foreach (var guild in Guilds)
                Console.WriteLine($"[ID: {guild.id} Name: {guild.name} Owner: {guild.owner}]");

            await HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
