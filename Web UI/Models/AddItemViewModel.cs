using DB.Models.Items;

namespace Web_UI.Models
{
    public class AddItemViewModel
    {
        public ItemBase Item { get; set; } = new ItemBase();
        public List<string> Modifiers { get; set; } = new List<string>();
    }
}
