using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaintItemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PaintItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ColorHex",
                table: "PaintItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ColorFamily",
                table: "PaintItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentsPath",
                table: "PaintItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "AvailableStock",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "BatchTracking",
                table: "PaintItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "PaintItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "COGSAccount",
                table: "PaintItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "PaintItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CostMethod",
                table: "PaintItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "PaintItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PaintItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentStock",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DefaultBin",
                table: "PaintItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "PaintItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "PaintItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "PaintItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IncomeAccount",
                table: "PaintItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InventoryAssetAccount",
                table: "PaintItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "InventoryValue",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsHazardousMaterial",
                table: "PaintItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "PaintItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPurchaseDate",
                table: "PaintItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSaleDate",
                table: "PaintItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LotTracking",
                table: "PaintItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MSRP",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "PaintItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumStock",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumStock",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "PaintItems",
                type: "ntext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PreferredVendorId",
                table: "PaintItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseUnit",
                table: "PaintItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ReorderPoint",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReservedStock",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "PaintItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SalesTaxCategory",
                table: "PaintItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SalesUnit",
                table: "PaintItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "PaintItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UPCBarcode",
                table: "PaintItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UnitOfMeasure",
                table: "PaintItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "PaintItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PaintItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseName",
                table: "PaintItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "PaintItems",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "PaintItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AttachmentsPath", "AvailableStock", "BatchTracking", "Brand", "COGSAccount", "Category", "CostMethod", "CreatedAtUtc", "CreatedBy", "CurrentStock", "DefaultBin", "Dimensions", "ExpirationDate", "ImagePath", "IncomeAccount", "InventoryAssetAccount", "InventoryValue", "IsHazardousMaterial", "ItemType", "LastPurchaseDate", "LastSaleDate", "LotTracking", "MSRP", "Manufacturer", "MaximumStock", "MinimumStock", "Notes", "PreferredVendorId", "PurchasePrice", "PurchaseUnit", "ReorderPoint", "ReservedStock", "SKU", "SalesTaxCategory", "SalesUnit", "SellingPrice", "UPCBarcode", "UnitOfMeasure", "UpdatedAtUtc", "UpdatedBy", "WarehouseName", "Weight" },
                values: new object[] { "", 0m, false, "", "", "", "Average Cost", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0m, "", "", null, "", "", "", 0m, false, "Finished Product", null, null, false, 65m, "", 0m, 0m, "", null, 35m, "GAL", 0m, 0m, "SKU-PB-001", "", "GAL", 55m, "", "GAL", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", 0m });

            migrationBuilder.UpdateData(
                table: "PaintItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AttachmentsPath", "AvailableStock", "BatchTracking", "Brand", "COGSAccount", "Category", "CostMethod", "CreatedAtUtc", "CreatedBy", "CurrentStock", "DefaultBin", "Dimensions", "ExpirationDate", "ImagePath", "IncomeAccount", "InventoryAssetAccount", "InventoryValue", "IsHazardousMaterial", "ItemType", "LastPurchaseDate", "LastSaleDate", "LotTracking", "MSRP", "Manufacturer", "MaximumStock", "MinimumStock", "Notes", "PreferredVendorId", "PurchasePrice", "PurchaseUnit", "ReorderPoint", "ReservedStock", "SKU", "SalesTaxCategory", "SalesUnit", "SellingPrice", "UPCBarcode", "UnitOfMeasure", "UpdatedAtUtc", "UpdatedBy", "WarehouseName", "Weight" },
                values: new object[] { "", 0m, false, "", "", "", "Average Cost", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0m, "", "", null, "", "", "", 0m, false, "Finished Product", null, null, false, 58m, "", 0m, 0m, "", null, 30m, "GAL", 0m, 0m, "SKU-LR-002", "", "GAL", 48m, "", "GAL", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", 0m });

            migrationBuilder.CreateIndex(
                name: "IX_PaintItems_PreferredVendorId",
                table: "PaintItems",
                column: "PreferredVendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaintItems_Vendors_PreferredVendorId",
                table: "PaintItems",
                column: "PreferredVendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaintItems_Vendors_PreferredVendorId",
                table: "PaintItems");

            migrationBuilder.DropIndex(
                name: "IX_PaintItems_PreferredVendorId",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "AttachmentsPath",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "AvailableStock",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "BatchTracking",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "COGSAccount",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "CostMethod",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "CurrentStock",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "DefaultBin",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "IncomeAccount",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "InventoryAssetAccount",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "InventoryValue",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "IsHazardousMaterial",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "LastPurchaseDate",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "LastSaleDate",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "LotTracking",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "MSRP",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "MaximumStock",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "PreferredVendorId",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "PurchaseUnit",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "ReorderPoint",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "ReservedStock",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "SalesTaxCategory",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "SalesUnit",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "UPCBarcode",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasure",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "WarehouseName",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "PaintItems");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PaintItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ColorHex",
                table: "PaintItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "ColorFamily",
                table: "PaintItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
