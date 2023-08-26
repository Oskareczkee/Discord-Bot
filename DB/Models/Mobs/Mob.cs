using DB.Models.Servers;
using System.ComponentModel.DataAnnotations;

namespace DB.Models.Mobs
{
    public class Mob : Entity
    {

        /*Each mob is unique to server, different servers may have different items*/
        public ulong? GuildID { get; set; }
        public Server? Server { get; set; }

        [Required(ErrorMessage = "Please enter mob name")]
        public string Name { get; set; } = string.Empty;

        //to add some lore to monsters
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Gold Award")]
        public int GoldAward { get; set; } = 1;
        [Display(Name = "XP Award")]
        public int XPAward { get; set; } = 1;

        //resistance is in %
        public int Resistance { get; set; } = 0;
    }
}
