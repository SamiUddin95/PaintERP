using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVendorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Vendors");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Vendors",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Vendors",
                newName: "CreatedBy");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Vendors",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalDocumentsPath",
                table: "Vendors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AveragePaymentDays",
                table: "Vendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Vendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessCity",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessCountry",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessCounty",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessLicenseNumber",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Vendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessState",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessStreetAddress",
                table: "Vendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessSuiteApt",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessZipCode",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Vendors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactExtension",
                table: "Vendors",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactFirstName",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactJobTitle",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactLastName",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactMobilePhone",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficePhone",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Vendors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "CreditLimit",
                table: "Vendors",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Vendors",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DBA",
                table: "Vendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultDiscountPercent",
                table: "Vendors",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DefaultExpenseAccount",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultTaxRate",
                table: "Vendors",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DefaultWarehouse",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FederalTaxId",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InsuranceCertificatePath",
                table: "Vendors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternalNotes",
                table: "Vendors",
                type: "ntext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Is1099Vendor",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPreferredVendor",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPurchaseDate",
                table: "Vendors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadTimeDays",
                table: "Vendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LegalBusinessName",
                table: "Vendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumOrderQuantity",
                table: "Vendors",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningBalance",
                table: "Vendors",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTerms",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredPaymentMethod",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredShippingMethod",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoutingNumber",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SalesTaxCode",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SalesTaxExemptionNumber",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SameAsBusinessAddress",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShippingCity",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingState",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingStreetAddress",
                table: "Vendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingZipCode",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StateTaxId",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SwiftCode",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalBills",
                table: "Vendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPurchaseOrders",
                table: "Vendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Vendors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "VendorCategory",
                table: "Vendors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VendorContractPath",
                table: "Vendors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VendorId",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VendorRating",
                table: "Vendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "VendorSince",
                table: "Vendors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "VendorType",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "W9FormPath",
                table: "Vendors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AccountNumber", "AccountType", "AdditionalDocumentsPath", "AveragePaymentDays", "BankName", "BusinessCity", "BusinessCountry", "BusinessCounty", "BusinessLicenseNumber", "BusinessName", "BusinessState", "BusinessStreetAddress", "BusinessSuiteApt", "BusinessZipCode", "ContactEmail", "ContactExtension", "ContactFirstName", "ContactJobTitle", "ContactLastName", "ContactMobilePhone", "ContactOfficePhone", "CreatedAtUtc", "CreatedBy", "CreditLimit", "Currency", "DBA", "DefaultDiscountPercent", "DefaultExpenseAccount", "DefaultTaxRate", "DefaultWarehouse", "FederalTaxId", "Industry", "InsuranceCertificatePath", "InternalNotes", "Is1099Vendor", "IsActive", "IsPreferredVendor", "LastPurchaseDate", "LeadTimeDays", "LegalBusinessName", "MinimumOrderQuantity", "OpeningBalance", "PaymentTerms", "PreferredPaymentMethod", "PreferredShippingMethod", "RoutingNumber", "SalesTaxCode", "SalesTaxExemptionNumber", "SameAsBusinessAddress", "ShippingCity", "ShippingState", "ShippingStreetAddress", "ShippingZipCode", "StateTaxId", "SwiftCode", "TotalBills", "TotalPurchaseOrders", "UpdatedAtUtc", "UpdatedBy", "VendorCategory", "VendorContractPath", "VendorId", "VendorRating", "VendorSince", "VendorType", "W9FormPath" },
                values: new object[] { "", "", "", 0, "", "", "USA", "", "", "Titan Pigments", "", "", "", "", "orders@titanpigments.com", "", "", "", "", "", "", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0m, "USD", "", 0m, "", 0m, "", "", "", "", "", false, true, false, null, 0, "", 0m, 0m, "Net 30", "Check", "", "", "", "", true, "", "", "", "", "", "", 0, 0, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Raw Materials", "", "VND26-0001", 5, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supplier", "" });

            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AccountNumber", "AccountType", "AdditionalDocumentsPath", "AveragePaymentDays", "BankName", "BusinessCity", "BusinessCountry", "BusinessCounty", "BusinessLicenseNumber", "BusinessName", "BusinessState", "BusinessStreetAddress", "BusinessSuiteApt", "BusinessZipCode", "ContactEmail", "ContactExtension", "ContactFirstName", "ContactJobTitle", "ContactLastName", "ContactMobilePhone", "ContactOfficePhone", "CreatedAtUtc", "CreatedBy", "CreditLimit", "Currency", "DBA", "DefaultDiscountPercent", "DefaultExpenseAccount", "DefaultTaxRate", "DefaultWarehouse", "FederalTaxId", "Industry", "InsuranceCertificatePath", "InternalNotes", "Is1099Vendor", "IsActive", "IsPreferredVendor", "LastPurchaseDate", "LeadTimeDays", "LegalBusinessName", "MinimumOrderQuantity", "OpeningBalance", "PaymentTerms", "PreferredPaymentMethod", "PreferredShippingMethod", "RoutingNumber", "SalesTaxCode", "SalesTaxExemptionNumber", "SameAsBusinessAddress", "ShippingCity", "ShippingState", "ShippingStreetAddress", "ShippingZipCode", "StateTaxId", "SwiftCode", "TotalBills", "TotalPurchaseOrders", "UpdatedAtUtc", "UpdatedBy", "VendorCategory", "VendorContractPath", "VendorId", "VendorRating", "VendorSince", "VendorType", "W9FormPath" },
                values: new object[] { "", "", "", 0, "", "", "USA", "", "", "Northshore Packaging", "", "", "", "", "billing@northshorepack.com", "", "", "", "", "", "", new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0m, "USD", "", 0m, "", 0m, "", "", "", "", "", false, true, false, null, 0, "", 0m, 0m, "Net 30", "Check", "", "", "", "", true, "", "", "", "", "", "", 0, 0, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Packaging", "", "VND26-0002", 5, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supplier", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "AdditionalDocumentsPath",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "AveragePaymentDays",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BusinessCity",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BusinessCountry",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BusinessCounty",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BusinessLicenseNumber",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BusinessState",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BusinessStreetAddress",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BusinessSuiteApt",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "BusinessZipCode",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactExtension",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactFirstName",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactJobTitle",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactLastName",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactMobilePhone",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactOfficePhone",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CreditLimit",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "DBA",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "DefaultDiscountPercent",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "DefaultExpenseAccount",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "DefaultTaxRate",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "DefaultWarehouse",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "FederalTaxId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "InsuranceCertificatePath",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "InternalNotes",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Is1099Vendor",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "IsPreferredVendor",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "LastPurchaseDate",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "LeadTimeDays",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "LegalBusinessName",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "MinimumOrderQuantity",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "OpeningBalance",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "PaymentTerms",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "PreferredPaymentMethod",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "PreferredShippingMethod",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "RoutingNumber",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "SalesTaxCode",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "SalesTaxExemptionNumber",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "SameAsBusinessAddress",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ShippingCity",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ShippingState",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ShippingStreetAddress",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ShippingZipCode",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "StateTaxId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "SwiftCode",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "TotalBills",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "TotalPurchaseOrders",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorCategory",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorContractPath",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorRating",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorSince",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorType",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "W9FormPath",
                table: "Vendors");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Vendors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Vendors",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Email", "Name" },
                values: new object[] { "Raw Materials", "orders@titanpigments.com", "Titan Pigments" });

            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Email", "Name" },
                values: new object[] { "Packaging", "billing@northshorepack.com", "Northshore Packaging" });
        }
    }
}
