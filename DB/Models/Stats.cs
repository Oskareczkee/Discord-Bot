namespace DB.Models
{
    public abstract class Stats : DBObject
    {

       virtual public int Strength { get; set; } = 0;
       virtual public int Agility { get; set; } = 0;
       virtual public int Intelligence { get; set; } = 0;
       virtual public int Endurance { get; set; } = 0;
       virtual public int Luck { get; set; } = 0;
       virtual public int Armor { get; set; } = 0;
    }
}
