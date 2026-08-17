using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddSourceProductionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceProductionId",
                table: "PaintItems",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PaintItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "SourceProductionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "PaintItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "SourceProductionId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_PaintItems_SourceProductionId",
                table: "PaintItems",
                column: "SourceProductionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaintItems_PaintProductions_SourceProductionId",
                table: "PaintItems",
                column: "SourceProductionId",
                principalTable: "PaintProductions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaintItems_PaintProductions_SourceProductionId",
                table: "PaintItems");

            migrationBuilder.DropIndex(
                name: "IX_PaintItems_SourceProductionId",
                table: "PaintItems");

            migrationBuilder.DropColumn(
                name: "SourceProductionId",
                table: "PaintItems");
        }
    }
}
