using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "minDamage",
                table: "Items",
                newName: "MinDamage");

            migrationBuilder.RenameColumn(
                name: "maxDamage",
                table: "Items",
                newName: "MaxDamage");

            migrationBuilder.RenameColumn(
                name: "minDamage",
                table: "Item",
                newName: "MinDamage");

            migrationBuilder.RenameColumn(
                name: "maxDamage",
                table: "Item",
                newName: "MaxDamage");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemModifiers");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "WeaponTypes");

            migrationBuilder.RenameColumn(
                name: "MinDamage",
                table: "Items",
                newName: "minDamage");

            migrationBuilder.RenameColumn(
                name: "MaxDamage",
                table: "Items",
                newName: "maxDamage");

            migrationBuilder.RenameColumn(
                name: "MinDamage",
                table: "Item",
                newName: "minDamage");

            migrationBuilder.RenameColumn(
                name: "MaxDamage",
                table: "Item",
                newName: "maxDamage");
        }
    }
}
