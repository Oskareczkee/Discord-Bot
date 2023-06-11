using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Models.Items.Enums
{
    public class ModifiersDB : DBObject
    {
        public string Name { get; set; } = string.Empty;
    }

    internal class ModifiersDBConfiguration : IEntityTypeConfiguration<ModifiersDB>
    {
        public void Configure(EntityTypeBuilder<ModifiersDB> builder)
        {
            builder.HasData
            (
                new ModifiersDB { ID = 1, Name = "None" },
                new ModifiersDB { ID = 2, Name = "MeleeDamage" },
                new ModifiersDB { ID = 3, Name = "MagicDamage" },
                new ModifiersDB { ID = 4, Name = "MagicAttackChance" },
                new ModifiersDB { ID = 5, Name = "CriticalAttackChance" },
                new ModifiersDB { ID = 6, Name = "CriticalDamage" },
                new ModifiersDB { ID = 7, Name = "DodgeChance" },
                new ModifiersDB { ID = 8, Name = "Damage" }
             );
        }
    }
}
