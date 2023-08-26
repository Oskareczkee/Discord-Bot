namespace Web_UI.Models.Discord
{
    public class DiscordGuildMember
    {
        public string nick { get; set; } = string.Empty;
        public IEnumerable<string> Roles = new List<string>();
    }
}
