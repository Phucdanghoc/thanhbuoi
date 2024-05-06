using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiaHangKhoiDiem",
                table: "Chuyens");

            migrationBuilder.AlterColumn<int>(
                name: "Trangthai",
                table: "Chuyens",
                type: "int",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Trangthai",
                table: "Chuyens",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<double>(
                name: "GiaHangKhoiDiem",
                table: "Chuyens",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
