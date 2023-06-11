using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Models.Items.Enums
{
    public class ItemTypeDB : DBObject
    {
        public string Name { get; set; } = string.Empty;
    }

    internal class ItemTypeDBConfiguration : IEntityTypeConfiguration<ItemTypeDB>
    {
        public void Configure(EntityTypeBuilder<ItemTypeDB> builder)
        {
            builder.HasData
            (
                new ItemTypeDB { ID = 1, Name = "None" },
                new ItemTypeDB { ID = 2, Name = "Helmet" },
                new ItemTypeDB { ID = 3, Name = "Chestplate" },
                new ItemTypeDB { ID = 4, Name = "Gloves" },
                new ItemTypeDB { ID = 5, Name = "Shoes" },
                new ItemTypeDB { ID = 6, Name = "Weapon" },
                new ItemTypeDB { ID = 7, Name = "Ring" },
                new ItemTypeDB { ID = 8, Name = "Belt" },
                new ItemTypeDB { ID = 9, Name = "Necklace" },
                new ItemTypeDB { ID = 10, Name = "Extra" },
                new ItemTypeDB { ID = 11, Name = "Potion" },
                new ItemTypeDB { ID = 12, Name = "Miscellaneous" }
             );
        }
    }
}
