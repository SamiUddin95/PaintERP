using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompanyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM Companies WHERE Id = 1)
                BEGIN
                    INSERT INTO Companies (Id, Name, Industry, Country, PrimaryColor, LogoUrl)
                    VALUES (1, 'USA Paint ERP', 'Industrial Coatings', 'USA', '#0A5C9E', '/images/logos/painterp-logo.svg')
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Don't delete the company as it has foreign key dependencies
            // This migration is essentially one-way seeding
        }
    }
}
