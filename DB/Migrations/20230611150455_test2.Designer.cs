﻿// <auto-generated />
using System;
using DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DB.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20230611150455_test2")]
    partial class test2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DB.Models.Items.Enums.ItemTypeDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ItemTypes");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "None"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Helmet"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Chestplate"
                        },
                        new
                        {
                            ID = 4,
                            Name = "Gloves"
                        },
                        new
                        {
                            ID = 5,
                            Name = "Shoes"
                        },
                        new
                        {
                            ID = 6,
                            Name = "Weapon"
                        },
                        new
                        {
                            ID = 7,
                            Name = "Ring"
                        },
                        new
                        {
                            ID = 8,
                            Name = "Belt"
                        },
                        new
                        {
                            ID = 9,
                            Name = "Necklace"
                        },
                        new
                        {
                            ID = 10,
                            Name = "Extra"
                        },
                        new
                        {
                            ID = 11,
                            Name = "Potion"
                        },
                        new
                        {
                            ID = 12,
                            Name = "Miscellaneous"
                        });
                });

            modelBuilder.Entity("DB.Models.Items.Enums.ModifiersDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ItemModifiers");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "None"
                        },
                        new
                        {
                            ID = 2,
                            Name = "MeleeDamage"
                        },
                        new
                        {
                            ID = 3,
                            Name = "MagicDamage"
                        },
                        new
                        {
                            ID = 4,
                            Name = "MagicAttackChance"
                        },
                        new
                        {
                            ID = 5,
                            Name = "CriticalAttackChance"
                        },
                        new
                        {
                            ID = 6,
                            Name = "CriticalDamage"
                        },
                        new
                        {
                            ID = 7,
                            Name = "DodgeChance"
                        },
                        new
                        {
                            ID = 8,
                            Name = "Damage"
                        });
                });

            modelBuilder.Entity("DB.Models.Items.Enums.WeaponTypeDB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("WeaponTypes");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "None"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Range"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Melee"
                        },
                        new
                        {
                            ID = 4,
                            Name = "Magic"
                        });
                });

            modelBuilder.Entity("DB.Models.Items.Item", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Agility")
                        .HasColumnType("int");

                    b.Property<int>("Armor")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Endurance")
                        .HasColumnType("int");

                    b.Property<int>("Intelligence")
                        .HasColumnType("int");

                    b.Property<int>("Luck")
                        .HasColumnType("int");

                    b.Property<int>("MaxDamage")
                        .HasColumnType("int");

                    b.Property<int>("MinDamage")
                        .HasColumnType("int");

                    b.Property<string>("Modifiers")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("WeaponType")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Item");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("DB.Models.Items.ItemBase", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Agility")
                        .HasColumnType("int");

                    b.Property<int>("Armor")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Endurance")
                        .HasColumnType("int");

                    b.Property<int>("Intelligence")
                        .HasColumnType("int");

                    b.Property<int>("Luck")
                        .HasColumnType("int");

                    b.Property<int>("MaxDamage")
                        .HasColumnType("int");

                    b.Property<int>("MinDamage")
                        .HasColumnType("int");

                    b.Property<string>("Modifiers")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("WeaponType")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("DB.Models.Mobs.Mob", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Agility")
                        .HasColumnType("int");

                    b.Property<int>("Armor")
                        .HasColumnType("int");

                    b.Property<int>("BaseDMG")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Endurance")
                        .HasColumnType("int");

                    b.Property<int>("GoldAward")
                        .HasColumnType("int");

                    b.Property<int>("HP")
                        .HasColumnType("int");

                    b.Property<int>("Intelligence")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int>("Luck")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Resistance")
                        .HasColumnType("int");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.Property<int>("XPAward")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Mobs");
                });

            modelBuilder.Entity("DB.Models.Profiles.Profile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Agility")
                        .HasColumnType("int");

                    b.Property<int>("Armor")
                        .HasColumnType("int");

                    b.Property<int>("BaseDMG")
                        .HasColumnType("int");

                    b.Property<decimal>("DiscordID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("Endurance")
                        .HasColumnType("int");

                    b.Property<double>("Gold")
                        .HasColumnType("float");

                    b.Property<decimal>("GuildID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("HP")
                        .HasColumnType("int");

                    b.Property<int>("Intelligence")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int>("Luck")
                        .HasColumnType("int");

                    b.Property<int>("NextLevel")
                        .HasColumnType("int");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.Property<int>("XP")
                        .HasColumnType("int");

                    b.Property<DateTime>("lastFightTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("lastFreeRerollTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("lastQuestTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("nextFightTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("nextQuestTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("DB.Models.Items.EquipmentItem", b =>
                {
                    b.HasBaseType("DB.Models.Items.Item");

                    b.Property<int?>("ProfileID")
                        .HasColumnType("int");

                    b.HasIndex("ProfileID");

                    b.ToTable("ProfileEquipments");
                });

            modelBuilder.Entity("DB.Models.Items.ProfileItem", b =>
                {
                    b.HasBaseType("DB.Models.Items.Item");

                    b.Property<int?>("ProfileID")
                        .HasColumnType("int");

                    b.HasIndex("ProfileID");

                    b.ToTable("ProfileItems");
                });

            modelBuilder.Entity("DB.Models.Items.ShopItem", b =>
                {
                    b.HasBaseType("DB.Models.Items.Item");

                    b.Property<int?>("ProfileID")
                        .HasColumnType("int");

                    b.HasIndex("ProfileID");

                    b.ToTable("ProfileShops");
                });

            modelBuilder.Entity("DB.Models.Items.EquipmentItem", b =>
                {
                    b.HasOne("DB.Models.Items.Item", null)
                        .WithOne()
                        .HasForeignKey("DB.Models.Items.EquipmentItem", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB.Models.Profiles.Profile", "Profile")
                        .WithMany("Equipment")
                        .HasForeignKey("ProfileID");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("DB.Models.Items.ProfileItem", b =>
                {
                    b.HasOne("DB.Models.Items.Item", null)
                        .WithOne()
                        .HasForeignKey("DB.Models.Items.ProfileItem", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB.Models.Profiles.Profile", "Profile")
                        .WithMany("Items")
                        .HasForeignKey("ProfileID");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("DB.Models.Items.ShopItem", b =>
                {
                    b.HasOne("DB.Models.Items.Item", null)
                        .WithOne()
                        .HasForeignKey("DB.Models.Items.ShopItem", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB.Models.Profiles.Profile", "Profile")
                        .WithMany("ShopItems")
                        .HasForeignKey("ProfileID");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("DB.Models.Profiles.Profile", b =>
                {
                    b.Navigation("Equipment");

                    b.Navigation("Items");

                    b.Navigation("ShopItems");
                });
#pragma warning restore 612, 618
        }
    }
}