using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class AddGRNIdToBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GRNNumber",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<int>(
                name: "GRNId",
                table: "Bills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_GRNId",
                table: "Bills",
                column: "GRNId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_GoodsReceivedNotes_GRNId",
                table: "Bills",
                column: "GRNId",
                principalTable: "GoodsReceivedNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_GoodsReceivedNotes_GRNId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_GRNId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "GRNId",
                table: "Bills");

            migrationBuilder.AddColumn<string>(
                name: "GRNNumber",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 1,
                column: "GRNNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 2,
                column: "GRNNumber",
                value: null);
        }
    }
}
