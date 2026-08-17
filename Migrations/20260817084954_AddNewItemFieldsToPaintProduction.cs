using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddNewItemFieldsToPaintProduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CreateNewItem",
                table: "PaintProductions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "NewItemCalculatedUnitCost",
                table: "PaintProductions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewItemCategory",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewItemDescription",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewItemName",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewItemSKU",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NewItemSellingPrice",
                table: "PaintProductions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewItemUnitOfMeasure",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPrice",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateNewItem",
                table: "PaintProductions");

            migrationBuilder.DropColumn(
                name: "NewItemCalculatedUnitCost",
                table: "PaintProductions");

            migrationBuilder.DropColumn(
                name: "NewItemCategory",
                table: "PaintProductions");

            migrationBuilder.DropColumn(
                name: "NewItemDescription",
                table: "PaintProductions");

            migrationBuilder.DropColumn(
                name: "NewItemName",
                table: "PaintProductions");

            migrationBuilder.DropColumn(
                name: "NewItemSKU",
                table: "PaintProductions");

            migrationBuilder.DropColumn(
                name: "NewItemSellingPrice",
                table: "PaintProductions");

            migrationBuilder.DropColumn(
                name: "NewItemUnitOfMeasure",
                table: "PaintProductions");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPrice",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
