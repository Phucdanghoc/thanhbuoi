using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Trangthai",
                table: "Xes",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "TrongTaiHang",
                table: "Xes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "Ves",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "Hanhli",
                table: "Ves",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrongTaiHang",
                table: "Xes");

            migrationBuilder.DropColumn(
                name: "Hanhli",
                table: "Ves");

            migrationBuilder.AlterColumn<bool>(
                name: "Trangthai",
                table: "Xes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "TrangThai",
                table: "Ves",
                type: "bit",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
