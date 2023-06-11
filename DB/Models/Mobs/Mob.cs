namespace DB.Models.Mobs
{
    public class Mob : Entity
    {

        public string Name { get; set; }

        //to add some lore to monsters
        public string Description { get; set; }
        public int GoldAward { get; set; } = 1;
        public int XPAward { get; set; } = 1;

        //resistance is in %
        public int Resistance { get; set; } = 0;
    }
}
