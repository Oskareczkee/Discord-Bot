using DB.Models.Items.Enums;
using DB.Models.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

/*
   Item might look pretty weird, especially ProfileItem
   EF Core forces links between types, so when i add an Item to ProfileItem
   it forces it to link that Item to the Item database
   I didn't want items to be linked, i wanted to players to have multiple same items in their inventory
   As well i wanted to same items have different stats depending on level etc. EF Core did not allow me to do so
   So that's why ProfileItem is pretty much item but with different name
 */

namespace DB.Models.Items
{
    public class ProfileItem : Item
    {
        public int? ProfileID { get; set; }
        public Profile? Profile { get; set; }
        public ProfileItem()
        {
            Name = "None";
            Description = "None";
            Price = 0;
            Type = ItemType.Miscellaneous;
            ID = 0;
        }

        public ProfileItem(int profileID, IItem item) : base(item)
        {
            ProfileID = profileID;
        }
    }
}
