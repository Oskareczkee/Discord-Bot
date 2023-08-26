using DB.Models.Items;
using DB.Models.Mobs;
using DB.Models.Profiles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models.Servers
{
    public class Server
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] //this allows us to set key by ourselves, since discord guildIDs are unique, this is pretty normal
        public ulong? GuildID { get; set; }

        public List<ItemBase> Items { get; set; } = new List<ItemBase>();
        public List<Profile> Profiles { get; set; } = new List<Profile>();
        public List<Mob> Mobs { get; set; } = new List<Mob>();
    }
}
