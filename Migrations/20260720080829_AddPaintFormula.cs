using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddPaintFormula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaintFormulas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    FormulaName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FormulaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaintColor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Finish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContainerSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalFormulaCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ExpectedYield = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    WastePercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    GrossMargin = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ParentFormulaId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintFormulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaintFormulas_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintFormulas_PaintFormulas_ParentFormulaId",
                        column: x => x.ParentFormulaId,
                        principalTable: "PaintFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaintFormulaItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaintFormulaId = table.Column<int>(type: "int", nullable: false),
                    PaintItemId = table.Column<int>(type: "int", nullable: true),
                    RawMaterialName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    RequiredQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InventoryAvailable = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintFormulaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaintFormulaItems_PaintFormulas_PaintFormulaId",
                        column: x => x.PaintFormulaId,
                        principalTable: "PaintFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaintFormulaItems_PaintItems_PaintItemId",
                        column: x => x.PaintItemId,
                        principalTable: "PaintItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaintFormulaItems_PaintFormulaId",
                table: "PaintFormulaItems",
                column: "PaintFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintFormulaItems_PaintItemId",
                table: "PaintFormulaItems",
                column: "PaintItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintFormulas_CompanyId",
                table: "PaintFormulas",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintFormulas_ParentFormulaId",
                table: "PaintFormulas",
                column: "ParentFormulaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaintFormulaItems");

            migrationBuilder.DropTable(
                name: "PaintFormulas");
        }
    }
}
