using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddStockLedgerAndSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompanyPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CompanyTaxId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CompanyLogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FiscalYearStart = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FiscalYearEnd = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DefaultCurrency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultPaymentTerms = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountingMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultTaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultTaxPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ShippingCarriers = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Currencies = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ApprovalWorkflow = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    InvoiceEmailTemplate = table.Column<string>(type: "ntext", nullable: false),
                    PaymentReceiptTemplate = table.Column<string>(type: "ntext", nullable: false),
                    PurchaseOrderEmailTemplate = table.Column<string>(type: "ntext", nullable: false),
                    BarcodeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvoiceNumberPrefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PurchaseOrderNumberPrefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BillNumberPrefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransferNumberPrefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuickBooksApiKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ShopifyApiKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AmazonApiKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpsApiKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FedExApiKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UspsApiKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSettings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockLedgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    PaintItemId = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InQty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OutQty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RunningBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockLedgers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockLedgers_PaintItems_PaintItemId",
                        column: x => x.PaintItemId,
                        principalTable: "PaintItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockLedgers_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_CompanyId",
                table: "AppSettings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_CompanyId",
                table: "StockLedgers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_PaintItemId",
                table: "StockLedgers",
                column: "PaintItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_WarehouseId",
                table: "StockLedgers",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "StockLedgers");
        }
    }
}
