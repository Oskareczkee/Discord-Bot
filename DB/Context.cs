using DB.Models.Items;
using DB.Models.Items.Enums;
using DB.Models.Mobs;
using DB.Models.Profiles;
using DB.Models.Servers;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<ItemBase> Items { get; set; } = null!;
        public DbSet<ProfileItem> ProfileItems { get; set; } = null!;
        public DbSet<EquipmentItem> ProfileEquipments { get; set; } = null!;
        public DbSet<ShopItem> ProfileShops { get; set; } = null!;
        public DbSet<Mob> Mobs { get; set; } = null!;
        public DbSet<Server> Servers { get; set; } = null!;
        public DbSet<ServerMusicSettings> ServerMusic { get; set; } = null!;
        public DbSet<WeaponTypeDB> WeaponTypes { get; set; } = null!;
        public DbSet<ItemTypeDB> ItemTypes { get; set; } = null!;
        public DbSet<ModifiersDB> ItemModifiers { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().UseTptMappingStrategy();
            modelBuilder.ApplyConfiguration(new WeaponTypeDBConfiguration());
            modelBuilder.ApplyConfiguration(new ItemTypeDBConfiguration());
            modelBuilder.ApplyConfiguration(new ModifiersDBConfiguration());

        }
    }

}
