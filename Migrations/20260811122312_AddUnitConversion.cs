using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "PaintProductionMaterials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UnitConversions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitConversions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitConversions");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "PaintProductionMaterials");
        }
    }
}
