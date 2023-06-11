using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Models.Items.Enums
{
    public class WeaponTypeDB : DBObject
    {
        public string Name { get; set; } = string.Empty;
    }

    internal class WeaponTypeDBConfiguration : IEntityTypeConfiguration<WeaponTypeDB>
    {
        public void Configure(EntityTypeBuilder<WeaponTypeDB> builder)
        {
            builder.HasData
            (
                new WeaponTypeDB { ID = 1, Name = "None" },
                new WeaponTypeDB { ID = 2, Name = "Range" },
                new WeaponTypeDB { ID = 3, Name = "Melee" },
                new WeaponTypeDB { ID = 4, Name = "Magic" }
             );
        }
    }
}
