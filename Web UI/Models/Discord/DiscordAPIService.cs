using DB;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Web_UI.Models.Discord
{
    public class DiscordAPIService : IDiscordAPIService
    {
        private IHttpContextAccessor context { get; } = null!;
        private readonly Context DbContext = null!;

        public DiscordAPIService(IHttpContextAccessor contextAccessor, Context DbContextAccessor)
        {
            context = contextAccessor;
            DbContext = DbContextAccessor;
        }

        private async Task<HttpResponseMessage?> GetDiscordResponse(string Uri)
        {
            var token = await context.HttpContext!.GetTokenAsync("access_token");

            if (string.IsNullOrEmpty(token))
                return null;

            using var client = new HttpClient();

            client.BaseAddress = new Uri("https://discordapp.com/api/");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            return await client.GetAsync(Uri);
        }

        public async Task<DiscordUser> GetDiscordUser()
        {
            var response = await GetDiscordResponse("users/@me");

            if (response == null)
                return new DiscordUser();

            response.EnsureSuccessStatusCode();

            DiscordUser user = JsonSerializer.Deserialize<DiscordUser>(await response.Content.ReadAsStringAsync()) ?? new DiscordUser();
            return user;
        }

        public async Task<List<DiscordGuild>> GetDiscordUserGuilds()
        {
            var response = await GetDiscordResponse("users/@me/guilds");

            if (response == null)
                return new List<DiscordGuild>();

            response.EnsureSuccessStatusCode();

            List<DiscordGuild> guilds = JsonSerializer.Deserialize<List<DiscordGuild>>(await response.Content.ReadAsStringAsync()) ?? new List<DiscordGuild>();
            return guilds;
        }

        public async Task<List<DiscordGuild>> GetDiscordUserOwnerGuilds()
        {
            var response = await GetDiscordResponse("users/@me/guilds");

            if (response == null)
                return new List<DiscordGuild>();

            response.EnsureSuccessStatusCode();

            List<DiscordGuild> guilds = JsonSerializer.Deserialize<List<DiscordGuild>>(await response.Content.ReadAsStringAsync()) ?? new List<DiscordGuild>();
            guilds = guilds.Where(g => g.owner).ToList();
            return guilds;
        }

        /// <summary>
        /// Gets all guilds, on which user has owner, and which have bot added on them
        /// </summary>
        public async Task<List<DiscordGuild>> GetDiscordUserOwnerGuildsWithBot()
        {
            var OwnerGuilds = await GetDiscordUserOwnerGuilds();
            List<DiscordGuild> output = new List<DiscordGuild>();
            foreach(var guild in OwnerGuilds)
            {
                if (DbContext.Servers.Any(s => s.GuildID == guild.id))
                    output.Add(guild);
            }

            return output;
        }

        /// <summary>
        /// Gets information about the user in given guild
        /// Information include nickname and roles<br />
        /// DO NOT SPAM THIS METHOD, OTHERWISE RESPONSE WILL HAVE 429 CODE, WHICH MAY LEAD TO THE BAN FROM DISCORD API
        /// </summary>
        public async Task<DiscordGuildMember> GetDiscordUserGuildInfo(long guildID)
        {
            var response = await GetDiscordResponse($"users/@me/guilds/{guildID}/member");

            if (response == null)
                return new DiscordGuildMember();
            response.EnsureSuccessStatusCode();

            DiscordGuildMember member = JsonSerializer.Deserialize<DiscordGuildMember>(await response.Content.ReadAsStringAsync()) ?? new DiscordGuildMember();

            return member;
        }
    }
}
