using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Customers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "LifetimeValue",
                table: "Customers",
                newName: "TotalSales");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Customers",
                newName: "CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "Industry",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "AverageOrderValue",
                table: "Customers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "BillingCity",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingCountry",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingCounty",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingState",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingStreetAddress",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingSuite",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingZipCode",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Customers",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactExtension",
                table: "Customers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactFirstName",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactLastName",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactMobile",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficePhone",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactTitle",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContractPath",
                table: "Customers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreditApplicationPath",
                table: "Customers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CreditLimit",
                table: "Customers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Customers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CustomerDiscountPercent",
                table: "Customers",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CustomerGroup",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CustomerRating",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CustomerSince",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CustomerType",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DBA",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DefaultPriceList",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryInstructions",
                table: "Customers",
                type: "ntext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryMethod",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FavoriteProducts",
                table: "Customers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FederalTaxId",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternalNotes",
                table: "Customers",
                type: "ntext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTaxExempt",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVIP",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastInvoiceDate",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPaymentDate",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalCompanyName",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "LifetimeRevenue",
                table: "Customers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "LoadingDockInfo",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningBalance",
                table: "Customers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "OtherDocumentsPath",
                table: "Customers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OutstandingBalance",
                table: "Customers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTerms",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredColorCodes",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredPaintBrand",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredPaymentMethod",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredWarehouse",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectType",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceivingHours",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResaleCertificateNumber",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResaleCertificatePath",
                table: "Customers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SalesRepresentative",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SalesTaxCode",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SameAsBillingAddress",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShippingCity",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingState",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingStreetAddress",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingZipCode",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AverageOrderValue", "BillingCity", "BillingCountry", "BillingCounty", "BillingState", "BillingStreetAddress", "BillingSuite", "BillingZipCode", "BusinessName", "ContactEmail", "ContactExtension", "ContactFirstName", "ContactLastName", "ContactMobile", "ContactOfficePhone", "ContactTitle", "ContractPath", "CreatedAtUtc", "CreatedBy", "CreditApplicationPath", "CreditLimit", "Currency", "CustomerDiscountPercent", "CustomerGroup", "CustomerId", "CustomerRating", "CustomerSince", "CustomerType", "DBA", "DefaultPriceList", "DeliveryInstructions", "DeliveryMethod", "FavoriteProducts", "FederalTaxId", "InternalNotes", "IsActive", "IsTaxExempt", "IsVIP", "LastInvoiceDate", "LastPaymentDate", "LegalCompanyName", "LifetimeRevenue", "LoadingDockInfo", "OpeningBalance", "OtherDocumentsPath", "OutstandingBalance", "PaymentTerms", "PreferredColorCodes", "PreferredPaintBrand", "PreferredPaymentMethod", "PreferredWarehouse", "ProjectType", "ReceivingHours", "ResaleCertificateNumber", "ResaleCertificatePath", "SalesRepresentative", "SalesTaxCode", "SameAsBillingAddress", "ShippingCity", "ShippingState", "ShippingStreetAddress", "ShippingZipCode", "TotalSales", "UpdatedAtUtc", "UpdatedBy" },
                values: new object[] { 0m, "", "USA", "", "", "", "", "", "Coastal Builders", "finance@coastalbuilders.us", "", "", "", "", "", "", "", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", 0m, "USD", 0m, "", "CST26-0001", 5, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Commercial", "", "", "", "", "", "", "", true, false, false, null, null, "", 560000m, "", 0m, "", 0m, "Net 30", "", "", "Check", "", "", "", "", "", "", "", true, "", "", "", "", 0m, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AverageOrderValue", "BillingCity", "BillingCountry", "BillingCounty", "BillingState", "BillingStreetAddress", "BillingSuite", "BillingZipCode", "BusinessName", "ContactEmail", "ContactExtension", "ContactFirstName", "ContactLastName", "ContactMobile", "ContactOfficePhone", "ContactTitle", "ContractPath", "CreatedAtUtc", "CreatedBy", "CreditApplicationPath", "CreditLimit", "Currency", "CustomerDiscountPercent", "CustomerGroup", "CustomerId", "CustomerRating", "CustomerSince", "CustomerType", "DBA", "DefaultPriceList", "DeliveryInstructions", "DeliveryMethod", "FavoriteProducts", "FederalTaxId", "InternalNotes", "IsActive", "IsTaxExempt", "IsVIP", "LastInvoiceDate", "LastPaymentDate", "LegalCompanyName", "LifetimeRevenue", "LoadingDockInfo", "OpeningBalance", "OtherDocumentsPath", "OutstandingBalance", "PaymentTerms", "PreferredColorCodes", "PreferredPaintBrand", "PreferredPaymentMethod", "PreferredWarehouse", "ProjectType", "ReceivingHours", "ResaleCertificateNumber", "ResaleCertificatePath", "SalesRepresentative", "SalesTaxCode", "SameAsBillingAddress", "ShippingCity", "ShippingState", "ShippingStreetAddress", "ShippingZipCode", "TotalSales", "UpdatedAtUtc", "UpdatedBy" },
                values: new object[] { 0m, "", "USA", "", "", "", "", "", "Evergreen Retail", "ap@evergreenretail.com", "", "", "", "", "", "", "", new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", 0m, "USD", 0m, "", "CST26-0002", 5, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Commercial", "", "", "", "", "", "", "", true, false, false, null, null, "", 420000m, "", 0m, "", 0m, "Net 30", "", "", "Check", "", "", "", "", "", "", "", true, "", "", "", "", 0m, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageOrderValue",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BillingCity",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BillingCountry",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BillingCounty",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BillingState",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BillingStreetAddress",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BillingSuite",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BillingZipCode",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContactExtension",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContactFirstName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContactLastName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContactMobile",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContactOfficePhone",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContactTitle",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContractPath",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreditApplicationPath",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreditLimit",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerDiscountPercent",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerGroup",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerRating",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerSince",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerType",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DBA",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DefaultPriceList",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeliveryInstructions",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeliveryMethod",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FavoriteProducts",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FederalTaxId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "InternalNotes",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsTaxExempt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsVIP",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastInvoiceDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastPaymentDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LegalCompanyName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LifetimeRevenue",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LoadingDockInfo",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "OpeningBalance",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "OtherDocumentsPath",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "OutstandingBalance",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PaymentTerms",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PreferredColorCodes",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PreferredPaintBrand",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PreferredPaymentMethod",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PreferredWarehouse",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ProjectType",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ReceivingHours",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ResaleCertificateNumber",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ResaleCertificatePath",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SalesRepresentative",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SalesTaxCode",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SameAsBillingAddress",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ShippingCity",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ShippingState",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ShippingStreetAddress",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ShippingZipCode",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Customers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TotalSales",
                table: "Customers",
                newName: "LifetimeValue");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Customers",
                newName: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "Industry",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "LifetimeValue", "Name" },
                values: new object[] { "finance@coastalbuilders.us", 560000m, "Coastal Builders" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "LifetimeValue", "Name" },
                values: new object[] { "ap@evergreenretail.com", 420000m, "Evergreen Retail" });
        }
    }
}
