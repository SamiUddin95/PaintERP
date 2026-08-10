using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintERP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItemDocumentFieldsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "PaintItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentsPath",
                table: "PaintItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                table: "PaintItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AttachmentsPath", "ImagePath" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PaintItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AttachmentsPath", "ImagePath" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "PaintItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentsPath",
                table: "PaintItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PaintItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AttachmentsPath", "ImagePath" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "PaintItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AttachmentsPath", "ImagePath" },
                values: new object[] { "", "" });
        }
    }
}
