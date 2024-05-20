using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HangGuis_DonHangChiTiets_ID_DonHangChiTiet",
                table: "HangGuis");

            migrationBuilder.DropIndex(
                name: "IX_HangGuis_ID_DonHangChiTiet",
                table: "HangGuis");

            migrationBuilder.DropColumn(
                name: "ID_DonHangChiTiet",
                table: "HangGuis");

            migrationBuilder.DropColumn(
                name: "LoaiDonHang",
                table: "DonHangChiTiets");

            migrationBuilder.AddColumn<int>(
                name: "ID_HangGui",
                table: "DonHangChiTiets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DonHangChiTiets_ID_HangGui",
                table: "DonHangChiTiets",
                column: "ID_HangGui");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangChiTiets_HangGuis_ID_HangGui",
                table: "DonHangChiTiets",
                column: "ID_HangGui",
                principalTable: "HangGuis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangChiTiets_HangGuis_ID_HangGui",
                table: "DonHangChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_DonHangChiTiets_ID_HangGui",
                table: "DonHangChiTiets");

            migrationBuilder.DropColumn(
                name: "ID_HangGui",
                table: "DonHangChiTiets");

            migrationBuilder.AddColumn<int>(
                name: "ID_DonHangChiTiet",
                table: "HangGuis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LoaiDonHang",
                table: "DonHangChiTiets",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_HangGuis_ID_DonHangChiTiet",
                table: "HangGuis",
                column: "ID_DonHangChiTiet");

            migrationBuilder.AddForeignKey(
                name: "FK_HangGuis_DonHangChiTiets_ID_DonHangChiTiet",
                table: "HangGuis",
                column: "ID_DonHangChiTiet",
                principalTable: "DonHangChiTiets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
