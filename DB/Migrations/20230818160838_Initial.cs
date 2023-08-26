using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    WeaponType = table.Column<int>(type: "int", nullable: false),
                    MinDamage = table.Column<int>(type: "int", nullable: false),
                    MaxDamage = table.Column<int>(type: "int", nullable: false),
                    Modifiers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Agility = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Endurance = table.Column<int>(type: "int", nullable: false),
                    Luck = table.Column<int>(type: "int", nullable: false),
                    Armor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ItemModifiers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemModifiers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    GuildID = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.GuildID);
                });

            migrationBuilder.CreateTable(
                name: "WeaponTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuildID = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    ServerGuildID = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    WeaponType = table.Column<int>(type: "int", nullable: false),
                    MinDamage = table.Column<int>(type: "int", nullable: false),
                    MaxDamage = table.Column<int>(type: "int", nullable: false),
                    Modifiers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Agility = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Endurance = table.Column<int>(type: "int", nullable: false),
                    Luck = table.Column<int>(type: "int", nullable: false),
                    Armor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Items_Servers_ServerGuildID",
                        column: x => x.ServerGuildID,
                        principalTable: "Servers",
                        principalColumn: "GuildID");
                });

            migrationBuilder.CreateTable(
                name: "Mobs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuildID = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    ServerGuildID = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoldAward = table.Column<int>(type: "int", nullable: false),
                    XPAward = table.Column<int>(type: "int", nullable: false),
                    Resistance = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Agility = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Endurance = table.Column<int>(type: "int", nullable: false),
                    Luck = table.Column<int>(type: "int", nullable: false),
                    Armor = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    HP = table.Column<int>(type: "int", nullable: false),
                    BaseDMG = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mobs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Mobs_Servers_ServerGuildID",
                        column: x => x.ServerGuildID,
                        principalTable: "Servers",
                        principalColumn: "GuildID");
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Agility = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Endurance = table.Column<int>(type: "int", nullable: false),
                    Luck = table.Column<int>(type: "int", nullable: false),
                    DiscordID = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    GuildID = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    ServerGuildID = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    XP = table.Column<int>(type: "int", nullable: false),
                    NextLevel = table.Column<int>(type: "int", nullable: false),
                    Gold = table.Column<double>(type: "float", nullable: false),
                    lastQuestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    nextQuestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastFightTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    nextFightTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastFreeRerollTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Armor = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    HP = table.Column<int>(type: "int", nullable: false),
                    BaseDMG = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Profiles_Servers_ServerGuildID",
                        column: x => x.ServerGuildID,
                        principalTable: "Servers",
                        principalColumn: "GuildID");
                });

            migrationBuilder.CreateTable(
                name: "ProfileEquipments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    ProfileID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileEquipments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProfileEquipments_Item_ID",
                        column: x => x.ID,
                        principalTable: "Item",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileEquipments_Profiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "Profiles",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ProfileItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    ProfileID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProfileItems_Item_ID",
                        column: x => x.ID,
                        principalTable: "Item",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileItems_Profiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "Profiles",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ProfileShops",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    ProfileID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileShops", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProfileShops_Item_ID",
                        column: x => x.ID,
                        principalTable: "Item",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileShops_Profiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "Profiles",
                        principalColumn: "ID");
                });

            migrationBuilder.InsertData(
                table: "ItemModifiers",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "None" },
                    { 2, "MeleeDamage" },
                    { 3, "MagicDamage" },
                    { 4, "MagicAttackChance" },
                    { 5, "CriticalAttackChance" },
                    { 6, "CriticalDamage" },
                    { 7, "DodgeChance" },
                    { 8, "Damage" }
                });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "None" },
                    { 2, "Helmet" },
                    { 3, "Chestplate" },
                    { 4, "Gloves" },
                    { 5, "Shoes" },
                    { 6, "Weapon" },
                    { 7, "Ring" },
                    { 8, "Belt" },
                    { 9, "Necklace" },
                    { 10, "Extra" },
                    { 11, "Potion" },
                    { 12, "Miscellaneous" }
                });

            migrationBuilder.InsertData(
                table: "WeaponTypes",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "None" },
                    { 2, "Range" },
                    { 3, "Melee" },
                    { 4, "Magic" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ServerGuildID",
                table: "Items",
                column: "ServerGuildID");

            migrationBuilder.CreateIndex(
                name: "IX_Mobs_ServerGuildID",
                table: "Mobs",
                column: "ServerGuildID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileEquipments_ProfileID",
                table: "ProfileEquipments",
                column: "ProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileItems_ProfileID",
                table: "ProfileItems",
                column: "ProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ServerGuildID",
                table: "Profiles",
                column: "ServerGuildID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileShops_ProfileID",
                table: "ProfileShops",
                column: "ProfileID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemModifiers");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "Mobs");

            migrationBuilder.DropTable(
                name: "ProfileEquipments");

            migrationBuilder.DropTable(
                name: "ProfileItems");

            migrationBuilder.DropTable(
                name: "ProfileShops");

            migrationBuilder.DropTable(
                name: "WeaponTypes");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}
