using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountingTaxUsersAndPostingFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockLedgers_PaintItemId",
                table: "StockLedgers");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_CustomerId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_VendorId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_Bills_VendorId",
                table: "Bills");

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "SalesInvoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    AccountCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NormalBalance = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemAccount = table.Column<bool>(type: "bit", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    EntryNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TotalDebit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCredit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaxRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    County = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(6,3)", precision: 6, scale: 3, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JournalEntryId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountCode", "AccountName", "AccountType", "Balance", "CompanyId", "CreatedAtUtc", "Description", "IsActive", "IsSystemAccount", "NormalBalance", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { 1, "1000", "Cash / Bank", "Asset", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Debit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "1010", "Undeposited Funds", "Asset", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Debit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "1200", "Accounts Receivable", "Asset", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Debit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "1300", "Inventory Asset", "Asset", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Debit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "2000", "Accounts Payable", "Liability", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Credit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "2200", "Sales Tax Payable", "Liability", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Credit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "4000", "Sales Revenue", "Income", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Credit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, "4100", "Shipping Income", "Income", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Credit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, "5000", "Cost of Goods Sold", "Expense", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Debit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, "5100", "Purchases / Freight", "Expense", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Debit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "5200", "Inventory Adjustment Expense", "Expense", 0m, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Debit", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "TaxRates",
                columns: new[] { "Id", "City", "CompanyId", "County", "CreatedAtUtc", "Description", "IsActive", "IsDefault", "Rate", "State", "TaxCode", "UpdatedAtUtc", "ZipCode" },
                values: new object[,]
                {
                    { 1, null, 1, null, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Texas State Sales Tax", true, true, 8.250m, "TX", "TX-STATE", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 2, null, 1, null, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Georgia State Sales Tax", true, false, 7.000m, "GA", "GA-STATE", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { 3, null, 1, null, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tax Exempt", true, false, 0m, null, "EXEMPT", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorPayments_PaymentNumber",
                table: "VendorPayments",
                column: "PaymentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_PaintItemId_TransactionDate",
                table: "StockLedgers",
                columns: new[] { "PaintItemId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_TransactionType",
                table: "StockLedgers",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_CustomerId_Status",
                table: "SalesInvoices",
                columns: new[] { "CustomerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_InvoiceDate",
                table: "SalesInvoices",
                column: "InvoiceDate");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_InvoiceNumber",
                table: "SalesInvoices",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PONumber",
                table: "PurchaseOrders",
                column: "PONumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorId_Status",
                table: "PurchaseOrders",
                columns: new[] { "VendorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductions_ProductionNumber",
                table: "PaintProductions",
                column: "ProductionNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaintFormulas_FormulaCode",
                table: "PaintFormulas",
                column: "FormulaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_TransferNumber",
                table: "InventoryTransfers",
                column: "TransferNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_AdjustmentNumber",
                table: "InventoryAdjustments",
                column: "AdjustmentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_PaymentNumber",
                table: "CustomerPayments",
                column: "PaymentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BillDate",
                table: "Bills",
                column: "BillDate");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BillNumber",
                table: "Bills",
                column: "BillNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_VendorId_Status",
                table: "Bills",
                columns: new[] { "VendorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CompanyId_AccountCode",
                table: "Accounts",
                columns: new[] { "CompanyId", "AccountCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_CompanyId",
                table: "AppUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_CompanyId",
                table: "JournalEntries",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_EntryNumber",
                table: "JournalEntries",
                column: "EntryNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_AccountId",
                table: "JournalEntryLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_JournalEntryId",
                table: "JournalEntryLines",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_TaxCode",
                table: "TaxRates",
                column: "TaxCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "JournalEntryLines");

            migrationBuilder.DropTable(
                name: "TaxRates");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_VendorPayments_PaymentNumber",
                table: "VendorPayments");

            migrationBuilder.DropIndex(
                name: "IX_StockLedgers_PaintItemId_TransactionDate",
                table: "StockLedgers");

            migrationBuilder.DropIndex(
                name: "IX_StockLedgers_TransactionType",
                table: "StockLedgers");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_CustomerId_Status",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_InvoiceDate",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_InvoiceNumber",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PONumber",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_VendorId_Status",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaintProductions_ProductionNumber",
                table: "PaintProductions");

            migrationBuilder.DropIndex(
                name: "IX_PaintFormulas_FormulaCode",
                table: "PaintFormulas");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransfers_TransferNumber",
                table: "InventoryTransfers");

            migrationBuilder.DropIndex(
                name: "IX_InventoryAdjustments_AdjustmentNumber",
                table: "InventoryAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPayments_PaymentNumber",
                table: "CustomerPayments");

            migrationBuilder.DropIndex(
                name: "IX_Bills_BillDate",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_BillNumber",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_VendorId_Status",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "Bills");

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_PaintItemId",
                table: "StockLedgers",
                column: "PaintItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_CustomerId",
                table: "SalesInvoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorId",
                table: "PurchaseOrders",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_VendorId",
                table: "Bills",
                column: "VendorId");
        }
    }
}
