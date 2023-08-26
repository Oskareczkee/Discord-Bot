using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Models.Servers
{
    public class ServerMusicSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] //this allows us to set key by ourselves, since discord guildIDs are unique, this is pretty normal
        public ulong? GuildID { get; set; }
        public int Volume { get; set; } = 100;
    }
}
