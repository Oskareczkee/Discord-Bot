using DB.Models.Items;
using DB.Models.Items.Enums;
using DB.Models.Mobs;
using DB.Models.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace DB
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ItemBase> Items { get; set; }
        public DbSet<ProfileItem> ProfileItems { get; set; }
        public DbSet<EquipmentItem> ProfileEquipments { get; set; }
        public DbSet<ShopItem> ProfileShops { get; set; }
        public DbSet<Mob> Mobs { get; set; }
        public DbSet<WeaponTypeDB> WeaponTypes { get; set; }
        public DbSet<ItemTypeDB> ItemTypes { get; set; }
        public DbSet<ModifiersDB> ItemModifiers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().UseTptMappingStrategy();
            modelBuilder.ApplyConfiguration(new WeaponTypeDBConfiguration());
            modelBuilder.ApplyConfiguration(new ItemTypeDBConfiguration());
            modelBuilder.ApplyConfiguration(new ModifiersDBConfiguration());

        }
    }

}
