namespace DB.Models
{
    public abstract class Entity : Stats
    {
        public int Level { get; set; } = 1;
        public int HP { get => Endurance * 2 * (Level + 1); }
        public int BaseDMG => Level * 2;
    }
}
