using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaritasaT.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "StorageItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShareURL",
                table: "StorageItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_FileName",
                table: "StorageItems",
                column: "FileName",
                unique: true,
                filter: "[FileName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StorageItems_FileName",
                table: "StorageItems");

            migrationBuilder.DropColumn(
                name: "ShareURL",
                table: "StorageItems");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "StorageItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
