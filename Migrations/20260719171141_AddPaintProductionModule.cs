using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddPaintProductionModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaintProductions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ProductionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Recipe = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    OutputQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishedProductId = table.Column<int>(type: "int", nullable: true),
                    FinishedProductDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MaterialCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LaborCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OverheadCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ProductionCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FinishedProductCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CostPerUnit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BatchLabel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QCStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QCReport = table.Column<string>(type: "ntext", nullable: true),
                    QCNotes = table.Column<string>(type: "ntext", nullable: true),
                    ProductionNotes = table.Column<string>(type: "ntext", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintProductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaintProductions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintProductions_PaintItems_FinishedProductId",
                        column: x => x.FinishedProductId,
                        principalTable: "PaintItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintProductions_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaintProductionMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaintProductionId = table.Column<int>(type: "int", nullable: false),
                    PaintItemId = table.Column<int>(type: "int", nullable: true),
                    MaterialName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequiredQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ConsumedQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PercentageInMix = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    StockBefore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StockAfter = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintProductionMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaintProductionMaterials_PaintItems_PaintItemId",
                        column: x => x.PaintItemId,
                        principalTable: "PaintItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintProductionMaterials_PaintProductions_PaintProductionId",
                        column: x => x.PaintProductionId,
                        principalTable: "PaintProductions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductionMaterials_PaintItemId",
                table: "PaintProductionMaterials",
                column: "PaintItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductionMaterials_PaintProductionId",
                table: "PaintProductionMaterials",
                column: "PaintProductionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductions_CompanyId",
                table: "PaintProductions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductions_FinishedProductId",
                table: "PaintProductions",
                column: "FinishedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductions_WarehouseId",
                table: "PaintProductions",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaintProductionMaterials");

            migrationBuilder.DropTable(
                name: "PaintProductions");
        }
    }
}
