using System.ComponentModel.DataAnnotations;

namespace DB.Models
{
    public abstract class Entity : Stats
    {
        public int Level { get; set; } = 1;

        [Display(Name = "Base HP")]
        [Range(1, int.MaxValue, ErrorMessage = "Base HP must be greater than 0")]
        public int HP { get; set; } = 1;

        [Display(Name = "Base DMG")]
        [Range(0, int.MaxValue, ErrorMessage = "Base DMG cannot be negative")]
        public int BaseDMG { get; set; } = 1;
    }
}
