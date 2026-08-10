using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddFormulaIdToPaintProduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormulaId",
                table: "PaintProductions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductions_FormulaId",
                table: "PaintProductions",
                column: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaintProductions_PaintFormulas_FormulaId",
                table: "PaintProductions",
                column: "FormulaId",
                principalTable: "PaintFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaintProductions_PaintFormulas_FormulaId",
                table: "PaintProductions");

            migrationBuilder.DropIndex(
                name: "IX_PaintProductions_FormulaId",
                table: "PaintProductions");

            migrationBuilder.DropColumn(
                name: "FormulaId",
                table: "PaintProductions");
        }
    }
}
