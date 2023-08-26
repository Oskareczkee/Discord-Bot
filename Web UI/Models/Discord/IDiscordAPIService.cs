namespace Web_UI.Models.Discord
{
    public interface IDiscordAPIService
    {
        public Task<DiscordUser> GetDiscordUser();
        public Task<List<DiscordGuild>> GetDiscordUserGuilds();
        public Task<List<DiscordGuild>> GetDiscordUserOwnerGuilds();
        public Task<DiscordGuildMember> GetDiscordUserGuildInfo(long guildID);
        public Task<List<DiscordGuild>> GetDiscordUserOwnerGuildsWithBot();
    }
}
