using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddComprehensiveFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSettings_Companies_CompanyId",
                table: "AppSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_BillPayments_Companies_CompanyId",
                table: "BillPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntryLines_Accounts_AccountId",
                table: "JournalEntryLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Bills_ConvertedBillId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Companies_CompanyId",
                table: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "InventoryAdjustmentItems");

            migrationBuilder.DropTable(
                name: "TaxRates");

            migrationBuilder.DropTable(
                name: "WarehouseStocks");

            migrationBuilder.DropTable(
                name: "InventoryAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_VendorPayments_VendorId",
                table: "VendorPayments");

            migrationBuilder.DropIndex(
                name: "IX_StockLedgers_PaintItemId_TransactionDate",
                table: "StockLedgers");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_CustomerId_Status",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ConvertedBillId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_VendorId_Status",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaintItems_WarehouseId",
                table: "PaintItems");

            migrationBuilder.DropIndex(
                name: "IX_PaintFormulas_FormulaCode",
                table: "PaintFormulas");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntryLines_AccountId",
                table: "JournalEntryLines");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPayments_CustomerId",
                table: "CustomerPayments");

            migrationBuilder.DropIndex(
                name: "IX_Bills_VendorId_Status",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppSettings_CompanyId",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "IsDefaultWarehouse",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "MapLocation",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "StorageBins",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseCode",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "VendorPayments");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SalesInvoices");

            // Skip RowVersion drop for PurchaseOrders - it's a timestamp column that cannot be altered
            // migrationBuilder.DropColumn(
            //     name: "RowVersion",
            //     table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "PaintProductions");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "InventoryTransfers");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "CustomerPayments");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "Debit",
                table: "JournalEntryLines",
                newName: "DebitAmount");

            migrationBuilder.RenameColumn(
                name: "Credit",
                table: "JournalEntryLines",
                newName: "CreditAmount");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "JournalEntryLines",
                newName: "LineNumber");

            migrationBuilder.RenameColumn(
                name: "ReferenceType",
                table: "JournalEntries",
                newName: "TransactionType");

            migrationBuilder.RenameColumn(
                name: "IsPosted",
                table: "JournalEntries",
                newName: "IsReversed");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "VendorPayments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InternalNotes",
                table: "SalesInvoices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerNotes",
                table: "SalesInvoices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VendorNotes",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PurchaseOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingAddress",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentTerms",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PONumber",
                table: "PurchaseOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "InternalNotes",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Buyer",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "PurchaseOrderItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "PurchaseOrderItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PurchaseOrderItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PaintProductions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Recipe",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "QCStatus",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QCReport",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QCNotes",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductionNumber",
                table: "PaintProductions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ProductionNotes",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FinishedProductDescription",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "BatchNumber",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "BatchLabel",
                table: "PaintProductions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaterialType",
                table: "PaintProductionMaterials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "MaterialName",
                table: "PaintProductionMaterials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "PaintFormulas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PaintFormulas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PaintColor",
                table: "PaintFormulas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FormulaName",
                table: "PaintFormulas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "FormulaCode",
                table: "PaintFormulas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Finish",
                table: "PaintFormulas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PaintFormulas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ContainerSize",
                table: "PaintFormulas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "PaintFormulaItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "RawMaterialName",
                table: "PaintFormulaItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "JournalEntryLines",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<string>(
                name: "AccountCode",
                table: "JournalEntryLines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "JournalEntryLines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                table: "JournalEntries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "JournalEntries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "ReversedByEntryId",
                table: "JournalEntries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReversedDate",
                table: "JournalEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "JournalEntries",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "JournalEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "JournalEntries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "InventoryTransfers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "CustomerPayments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseOrderEmailTemplate",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentReceiptTemplate",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceEmailTemplate",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultTaxCode",
                table: "AppSettings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalWorkflow",
                table: "AppSettings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            // Skip seed data insertion - database already has data
            // migrationBuilder.InsertData(
            //     table: "PurchaseOrders",
            //     columns: new[] { "Id", "AmountReceived", "ApprovedDate", "Buyer", "CancelledDate", "CompanyId", "ConvertedBillId", "CreatedAtUtc", "CreatedBy", "DiscountAmount", "ExpectedDeliveryDate", "InternalNotes", "IsApproved", "IsCancelled", "IsFullyReceived", "OrderDate", "PONumber", "PaymentTerms", "ReferenceNumber", "ShippingAddress", "ShippingCost", "Status", "Subtotal", "TaxAmount", "TotalAmount", "UpdatedAtUtc", "UpdatedBy", "VendorId", "VendorNotes", "WarehouseId" },
            //     values: new object[,]
            //     {
            //         { 1, 0m, null, "", null, 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m, null, null, false, false, false, new DateTime(2026, 6, 28, 0, 0, 0, 0, DateTimeKind.Utc), "", null, null, "", 0m, "Awaiting Approval", 0m, 0m, 29000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, null },
            //         { 2, 0m, null, "", null, 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m, null, null, false, false, false, new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), "", null, null, "", 0m, "Approved", 0m, 0m, 18000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null }
            //     });

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_ContactEmail",
                table: "Vendors",
                column: "ContactEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_IsActive",
                table: "Vendors",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_VendorId",
                table: "Vendors",
                column: "VendorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorPayments_PaymentDate",
                table: "VendorPayments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_VendorPayments_VendorId_PaymentDate",
                table: "VendorPayments",
                columns: new[] { "VendorId", "PaymentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_PaintItemId_WarehouseId_TransactionDate",
                table: "StockLedgers",
                columns: new[] { "PaintItemId", "WarehouseId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_TransactionDate",
                table: "StockLedgers",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_CustomerId_InvoiceDate",
                table: "SalesInvoices",
                columns: new[] { "CustomerId", "InvoiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_Status",
                table: "SalesInvoices",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_OrderDate",
                table: "PurchaseOrders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Status",
                table: "PurchaseOrders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorId_OrderDate",
                table: "PurchaseOrders",
                columns: new[] { "VendorId", "OrderDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductions_ProductionDate",
                table: "PaintProductions",
                column: "ProductionDate");

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductions_Status",
                table: "PaintProductions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaintItems_Name",
                table: "PaintItems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PaintItems_SKU",
                table: "PaintItems",
                column: "SKU");

            migrationBuilder.CreateIndex(
                name: "IX_PaintItems_WarehouseId_StockQuantity",
                table: "PaintItems",
                columns: new[] { "WarehouseId", "StockQuantity" });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_EntryDate",
                table: "JournalEntries",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_TransactionType",
                table: "JournalEntries",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_TransactionType_ReferenceId",
                table: "JournalEntries",
                columns: new[] { "TransactionType", "ReferenceId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_Status",
                table: "InventoryTransfers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_TransferDate",
                table: "InventoryTransfers",
                column: "TransferDate");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ContactEmail",
                table: "Customers",
                column: "ContactEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerId",
                table: "Customers",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IsActive",
                table: "Customers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_CustomerId_PaymentDate",
                table: "CustomerPayments",
                columns: new[] { "CustomerId", "PaymentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_PaymentDate",
                table: "CustomerPayments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_Status",
                table: "Bills",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_VendorId_BillDate",
                table: "Bills",
                columns: new[] { "VendorId", "BillDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_BillPayments_Companies_CompanyId",
                table: "BillPayments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Companies_CompanyId",
                table: "PurchaseOrders",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillPayments_Companies_CompanyId",
                table: "BillPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Companies_CompanyId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_ContactEmail",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_IsActive",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_VendorId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_VendorPayments_PaymentDate",
                table: "VendorPayments");

            migrationBuilder.DropIndex(
                name: "IX_VendorPayments_VendorId_PaymentDate",
                table: "VendorPayments");

            migrationBuilder.DropIndex(
                name: "IX_StockLedgers_PaintItemId_WarehouseId_TransactionDate",
                table: "StockLedgers");

            migrationBuilder.DropIndex(
                name: "IX_StockLedgers_TransactionDate",
                table: "StockLedgers");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_CustomerId_InvoiceDate",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_Status",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_OrderDate",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_Status",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_VendorId_OrderDate",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaintProductions_ProductionDate",
                table: "PaintProductions");

            migrationBuilder.DropIndex(
                name: "IX_PaintProductions_Status",
                table: "PaintProductions");

            migrationBuilder.DropIndex(
                name: "IX_PaintItems_Name",
                table: "PaintItems");

            migrationBuilder.DropIndex(
                name: "IX_PaintItems_SKU",
                table: "PaintItems");

            migrationBuilder.DropIndex(
                name: "IX_PaintItems_WarehouseId_StockQuantity",
                table: "PaintItems");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_EntryDate",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_TransactionType",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_TransactionType_ReferenceId",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransfers_Status",
                table: "InventoryTransfers");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransfers_TransferDate",
                table: "InventoryTransfers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ContactEmail",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CustomerId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_IsActive",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPayments_CustomerId_PaymentDate",
                table: "CustomerPayments");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPayments_PaymentDate",
                table: "CustomerPayments");

            migrationBuilder.DropIndex(
                name: "IX_Bills_Status",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_VendorId_BillDate",
                table: "Bills");

            migrationBuilder.DeleteData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "AccountCode",
                table: "JournalEntryLines");

            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "JournalEntryLines");

            migrationBuilder.DropColumn(
                name: "ReversedByEntryId",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "ReversedDate",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "JournalEntries");

            migrationBuilder.RenameColumn(
                name: "LineNumber",
                table: "JournalEntryLines",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "DebitAmount",
                table: "JournalEntryLines",
                newName: "Debit");

            migrationBuilder.RenameColumn(
                name: "CreditAmount",
                table: "JournalEntryLines",
                newName: "Credit");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "JournalEntries",
                newName: "ReferenceType");

            migrationBuilder.RenameColumn(
                name: "IsReversed",
                table: "JournalEntries",
                newName: "IsPosted");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Warehouses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Warehouses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Warehouses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Capacity",
                table: "Warehouses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Warehouses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Warehouses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultWarehouse",
                table: "Warehouses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "Warehouses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MapLocation",
                table: "Warehouses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Warehouses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Warehouses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StorageBins",
                table: "Warehouses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Warehouses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseCode",
                table: "Warehouses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "VendorPayments",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // Skip RowVersion for VendorPayments - keeping it removed
            // migrationBuilder.AddColumn<byte[]>(
            //     name: "RowVersion",
            //     table: "VendorPayments",
            //     type: "rowversion",
            //     rowVersion: true,
            //     nullable: false,
            //     defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "InternalNotes",
                table: "SalesInvoices",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerNotes",
                table: "SalesInvoices",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "SalesInvoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Skip RowVersion for SalesInvoices - keeping it removed
            // migrationBuilder.AddColumn<byte[]>(
            //     name: "RowVersion",
            //     table: "SalesInvoices",
            //     type: "rowversion",
            //     rowVersion: true,
            //     nullable: false,
            //     defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "VendorNotes",
                table: "PurchaseOrders",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PurchaseOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ShippingAddress",
                table: "PurchaseOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                table: "PurchaseOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentTerms",
                table: "PurchaseOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PONumber",
                table: "PurchaseOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "InternalNotes",
                table: "PurchaseOrders",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Buyer",
                table: "PurchaseOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Skip RowVersion for PurchaseOrders - keeping it removed
            // migrationBuilder.AddColumn<byte[]>(
            //     name: "RowVersion",
            //     table: "PurchaseOrders",
            //     type: "rowversion",
            //     rowVersion: true,
            //     nullable: false,
            //     defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "PurchaseOrderItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "PurchaseOrderItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PurchaseOrderItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PaintProductions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Recipe",
                table: "PaintProductions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "QCStatus",
                table: "PaintProductions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QCReport",
                table: "PaintProductions",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QCNotes",
                table: "PaintProductions",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductionNumber",
                table: "PaintProductions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProductionNotes",
                table: "PaintProductions",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FinishedProductDescription",
                table: "PaintProductions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BatchNumber",
                table: "PaintProductions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BatchLabel",
                table: "PaintProductions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // Skip RowVersion for PaintProductions - keeping it removed
            // migrationBuilder.AddColumn<byte[]>(
            //     name: "RowVersion",
            //     table: "PaintProductions",
            //     type: "rowversion",
            //     rowVersion: true,
            //     nullable: false,
            //     defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "MaterialType",
                table: "PaintProductionMaterials",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MaterialName",
                table: "PaintProductionMaterials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "PaintFormulas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PaintFormulas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PaintColor",
                table: "PaintFormulas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FormulaName",
                table: "PaintFormulas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FormulaCode",
                table: "PaintFormulas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Finish",
                table: "PaintFormulas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PaintFormulas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContainerSize",
                table: "PaintFormulas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "PaintFormulaItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RawMaterialName",
                table: "PaintFormulaItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "JournalEntryLines",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                table: "JournalEntries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "JournalEntries",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "InventoryTransfers",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // Skip RowVersion for InventoryTransfers - keeping it removed
            // migrationBuilder.AddColumn<byte[]>(
            //     name: "RowVersion",
            //     table: "InventoryTransfers",
            //     type: "rowversion",
            //     rowVersion: true,
            //     nullable: false,
            //     defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "CustomerPayments",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // Skip RowVersion for CustomerPayments - keeping it removed
            // migrationBuilder.AddColumn<byte[]>(
            //     name: "RowVersion",
            //     table: "CustomerPayments",
            //     type: "rowversion",
            //     rowVersion: true,
            //     nullable: false,
            //     defaultValue: new byte[0]);

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Skip RowVersion for Bills - keeping it removed
            // migrationBuilder.AddColumn<byte[]>(
            //     name: "RowVersion",
            //     table: "Bills",
            //     type: "rowversion",
            //     rowVersion: true,
            //     nullable: false,
            //     defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseOrderEmailTemplate",
                table: "AppSettings",
                type: "ntext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentReceiptTemplate",
                table: "AppSettings",
                type: "ntext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceEmailTemplate",
                table: "AppSettings",
                type: "ntext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultTaxCode",
                table: "AppSettings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalWorkflow",
                table: "AppSettings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

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
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemAccount = table.Column<bool>(type: "bit", nullable: false),
                    NormalBalance = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
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
                name: "InventoryAdjustments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    AdjustmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdjustmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdjustmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalNotes = table.Column<string>(type: "ntext", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    // RowVersion removed from entity
                    // RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    TotalAdjustmentValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCostImpact = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustments_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaxRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    County = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(6,3)", precision: 6, scale: 3, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    PaintItemId = table.Column<int>(type: "int", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    AvailableStock = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentStock = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InventoryValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastMovementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastMovementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaximumStock = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReorderLevel = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReservedStock = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseStocks_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseStocks_PaintItems_PaintItemId",
                        column: x => x.PaintItemId,
                        principalTable: "PaintItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseStocks_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAdjustmentItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryAdjustmentId = table.Column<int>(type: "int", nullable: false),
                    PaintItemId = table.Column<int>(type: "int", nullable: true),
                    AdjustedQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AdjustmentValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrentQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Difference = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAdjustmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustmentItems_InventoryAdjustments_InventoryAdjustmentId",
                        column: x => x.InventoryAdjustmentId,
                        principalTable: "InventoryAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustmentItems_PaintItems_PaintItemId",
                        column: x => x.PaintItemId,
                        principalTable: "PaintItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Capacity", "CreatedAtUtc", "CreatedBy", "Email", "IsDefaultWarehouse", "Manager", "MapLocation", "Phone", "Status", "StorageBins", "UpdatedAtUtc", "UpdatedBy", "WarehouseCode" },
                values: new object[] { "123 Main St, Dallas, TX", "100,000 Gallons", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "dallas@painterp.com", true, "John Smith", "https://maps.example.com/dallas", "214-555-0100", "Active", 50, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "WH-DAL-001" });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "Capacity", "CreatedAtUtc", "CreatedBy", "Email", "IsDefaultWarehouse", "Manager", "MapLocation", "Phone", "Status", "StorageBins", "UpdatedAtUtc", "UpdatedBy", "WarehouseCode" },
                values: new object[] { "456 Peachtree Rd, Atlanta, GA", "80,000 Gallons", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "atlanta@painterp.com", false, "Sarah Johnson", "https://maps.example.com/atlanta", "404-555-0200", "Active", 40, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "WH-ATL-002" });

            migrationBuilder.CreateIndex(
                name: "IX_VendorPayments_VendorId",
                table: "VendorPayments",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_PaintItemId_TransactionDate",
                table: "StockLedgers",
                columns: new[] { "PaintItemId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_CustomerId_Status",
                table: "SalesInvoices",
                columns: new[] { "CustomerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ConvertedBillId",
                table: "PurchaseOrders",
                column: "ConvertedBillId",
                unique: true,
                filter: "[ConvertedBillId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorId_Status",
                table: "PurchaseOrders",
                columns: new[] { "VendorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_PaintItems_WarehouseId",
                table: "PaintItems",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintFormulas_FormulaCode",
                table: "PaintFormulas",
                column: "FormulaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_AccountId",
                table: "JournalEntryLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_CustomerId",
                table: "CustomerPayments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_VendorId_Status",
                table: "Bills",
                columns: new[] { "VendorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_CompanyId",
                table: "AppSettings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CompanyId_AccountCode",
                table: "Accounts",
                columns: new[] { "CompanyId", "AccountCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustmentItems_InventoryAdjustmentId",
                table: "InventoryAdjustmentItems",
                column: "InventoryAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustmentItems_PaintItemId",
                table: "InventoryAdjustmentItems",
                column: "PaintItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_AdjustmentNumber",
                table: "InventoryAdjustments",
                column: "AdjustmentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_CompanyId",
                table: "InventoryAdjustments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_WarehouseId",
                table: "InventoryAdjustments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_TaxCode",
                table: "TaxRates",
                column: "TaxCode");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStocks_CompanyId",
                table: "WarehouseStocks",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStocks_PaintItemId",
                table: "WarehouseStocks",
                column: "PaintItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStocks_WarehouseId_PaintItemId",
                table: "WarehouseStocks",
                columns: new[] { "WarehouseId", "PaintItemId" },
                unique: true,
                filter: "[PaintItemId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSettings_Companies_CompanyId",
                table: "AppSettings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BillPayments_Companies_CompanyId",
                table: "BillPayments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntryLines_Accounts_AccountId",
                table: "JournalEntryLines",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Bills_ConvertedBillId",
                table: "PurchaseOrders",
                column: "ConvertedBillId",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Companies_CompanyId",
                table: "PurchaseOrders",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
