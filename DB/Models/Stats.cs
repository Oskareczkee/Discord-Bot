namespace DB.Models
{
    public abstract class Stats : DBObject
    {

        public int Strength { get; set; } = 0;
        public int Agility { get; set; } = 0;
        public int Intelligence { get; set; } = 0;
        public int Endurance { get; set; } = 0;
        public int Luck { get; set; } = 0;
        public int Armor { get; set; } = 0;
    }
}
