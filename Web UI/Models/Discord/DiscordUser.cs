/*
 * This class only contains information that is usefull for us, other information is ignored
 */

using System.Text.Json.Serialization;

namespace Web_UI.Models.Discord
{
    public class DiscordUser
    {
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long id { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string avatar { get; set; } = string.Empty;
    }
}
