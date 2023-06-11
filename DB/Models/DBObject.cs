using System.ComponentModel.DataAnnotations;

namespace DB.Models
{
    public abstract class DBObject
    {
        [Key]
        public int ID { get; set; }
    }
}
