using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddBillModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Bills",
                newName: "TaxAmount");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "Bills",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceDue",
                table: "Bills",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "BillNumber",
                table: "Bills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Bills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Bills",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Bills",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Bills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GrandTotal",
                table: "Bills",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "InternalNotes",
                table: "Bills",
                type: "ntext",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVoid",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherCharges",
                table: "Bills",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "Bills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTerms",
                table: "Bills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                table: "Bills",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "Bills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCharges",
                table: "Bills",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ShippingMethod",
                table: "Bills",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Bills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "Bills",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TaxCode",
                table: "Bills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Bills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VendorInvoiceNumber",
                table: "Bills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VendorNotes",
                table: "Bills",
                type: "ntext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Bills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    PaintItemId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillItems_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillItems_PaintItems_PaintItemId",
                        column: x => x.PaintItemId,
                        principalTable: "PaintItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillItems_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountTaken = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    WriteOffAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CheckNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillPayments_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillPayments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_PurchaseOrderId",
                table: "Bills",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_WarehouseId",
                table: "Bills",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_BillItems_BillId",
                table: "BillItems",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BillItems_PaintItemId",
                table: "BillItems",
                column: "PaintItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BillItems_WarehouseId",
                table: "BillItems",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_BillId",
                table: "BillPayments",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_CompanyId",
                table: "BillPayments",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_PurchaseOrders_PurchaseOrderId",
                table: "Bills",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Warehouses_WarehouseId",
                table: "Bills",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_PurchaseOrders_PurchaseOrderId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Warehouses_WarehouseId",
                table: "Bills");

            migrationBuilder.DropTable(
                name: "BillItems");

            migrationBuilder.DropTable(
                name: "BillPayments");

            migrationBuilder.DropIndex(
                name: "IX_Bills_PurchaseOrderId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_WarehouseId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "BalanceDue",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "BillNumber",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "GrandTotal",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "InternalNotes",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "IsVoid",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "OtherCharges",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PaymentTerms",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "ShippingCharges",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "ShippingMethod",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "TaxCode",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "VendorInvoiceNumber",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "VendorNotes",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "TaxAmount",
                table: "Bills",
                newName: "Amount");

            migrationBuilder.InsertData(
                table: "Bills",
                columns: new[] { "Id", "Amount", "BillDate", "CompanyId", "IsPaid", "VendorId" },
                values: new object[,]
                {
                    { 1, 25000m, new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1 },
                    { 2, 17000m, new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2 }
                });
        }
    }
}
