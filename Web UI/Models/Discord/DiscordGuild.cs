using System.Text.Json.Serialization;

namespace Web_UI.Models.Discord
{
    public class DiscordGuild
    {
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public ulong id { get; set; }
        public string name { get; set; } = string.Empty;
        public string icon { get; set; } = string.Empty;
        public bool owner { get; set; }
    }
}
