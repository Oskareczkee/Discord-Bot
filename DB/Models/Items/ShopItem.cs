using DB.Models.Items.Enums;
using DB.Models.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models.Items
{
    public class ShopItem : Item
    {
        public int? ProfileID { get; set; }
        public Profile? Profile { get; set; }

        public ShopItem()
        {
            Name = "None";
            Description = "None";
            Price = 0;
            Type = ItemType.Miscellaneous;
            ID = 0;
        }

        public ShopItem(int profileID, IItem item) : base(item)
        {
            ProfileID = profileID;
        }
    }
}
