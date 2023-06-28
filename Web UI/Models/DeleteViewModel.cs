namespace Web_UI.Models
{
    public class DeleteViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int ID { get; set; }

        //Type can be Item. Mob etc...
        public string Type { get; set; } = string.Empty;
    }
}
