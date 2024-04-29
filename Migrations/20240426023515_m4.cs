using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiaTuyens_AspNetUsers_ID_TaiKhoan",
                table: "GiaTuyens");

            migrationBuilder.DropIndex(
                name: "IX_GiaTuyens_ID_TaiKhoan",
                table: "GiaTuyens");

            migrationBuilder.DropColumn(
                name: "ID_TaiKhoan",
                table: "GiaTuyens");

            migrationBuilder.AddColumn<int>(
                name: "Ten",
                table: "GiaTuyens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GiaTuyens_Ten",
                table: "GiaTuyens",
                column: "Ten",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GiaTuyens_Ten",
                table: "GiaTuyens");

            migrationBuilder.DropColumn(
                name: "Ten",
                table: "GiaTuyens");

            migrationBuilder.AddColumn<string>(
                name: "ID_TaiKhoan",
                table: "GiaTuyens",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiaTuyens_ID_TaiKhoan",
                table: "GiaTuyens",
                column: "ID_TaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_GiaTuyens_AspNetUsers_ID_TaiKhoan",
                table: "GiaTuyens",
                column: "ID_TaiKhoan",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
