using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_AspNetUsers_TaiKhoanId",
                table: "DonHangs");

            migrationBuilder.RenameColumn(
                name: "TaiKhoanId",
                table: "DonHangs",
                newName: "ID_TaiKhoan");

            migrationBuilder.RenameIndex(
                name: "IX_DonHangs_TaiKhoanId",
                table: "DonHangs",
                newName: "IX_DonHangs_ID_TaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_AspNetUsers_ID_TaiKhoan",
                table: "DonHangs",
                column: "ID_TaiKhoan",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_AspNetUsers_ID_TaiKhoan",
                table: "DonHangs");

            migrationBuilder.RenameColumn(
                name: "ID_TaiKhoan",
                table: "DonHangs",
                newName: "TaiKhoanId");

            migrationBuilder.RenameIndex(
                name: "IX_DonHangs_ID_TaiKhoan",
                table: "DonHangs",
                newName: "IX_DonHangs_TaiKhoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_AspNetUsers_TaiKhoanId",
                table: "DonHangs",
                column: "TaiKhoanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
