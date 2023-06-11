using DB.Models.Items.Enums;
using DB.Models.Profiles;

namespace DB.Models.Items
{
    public class EquipmentItem : Item
    {
        public int? ProfileID { get; set; }
        public Profile? Profile { get; set; }

        public EquipmentItem() : base() { }
        public EquipmentItem(int profileID, ItemType type)
        {
            ProfileID = profileID;

            Name = "None";
            Description = "None";
            Price = 0;
            Type = type;
        }

        public EquipmentItem(int profileID, IItem item) : base(item)
        {
            ProfileID = profileID;
        }
    }
}
