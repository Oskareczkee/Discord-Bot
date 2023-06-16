namespace DB.Models
{
    public abstract class Entity : Stats
    {
        public int Level { get; set; } = 1;
        public int HP { get; set; } = 1;
        public int BaseDMG { get; set; } = 1;
    }
}
