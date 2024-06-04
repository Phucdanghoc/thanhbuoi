using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangChiTiets_HangGuis_ID_HangGui",
                table: "DonHangChiTiets");

            migrationBuilder.AddColumn<bool>(
                name: "isCancel",
                table: "Ves",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ID_HangGui",
                table: "DonHangChiTiets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ID_Ve",
                table: "DonHangChiTiets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonHangChiTiets_ID_Ve",
                table: "DonHangChiTiets",
                column: "ID_Ve");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangChiTiets_HangGuis_ID_HangGui",
                table: "DonHangChiTiets",
                column: "ID_HangGui",
                principalTable: "HangGuis",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangChiTiets_Ves_ID_Ve",
                table: "DonHangChiTiets",
                column: "ID_Ve",
                principalTable: "Ves",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangChiTiets_HangGuis_ID_HangGui",
                table: "DonHangChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangChiTiets_Ves_ID_Ve",
                table: "DonHangChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_DonHangChiTiets_ID_Ve",
                table: "DonHangChiTiets");

            migrationBuilder.DropColumn(
                name: "isCancel",
                table: "Ves");

            migrationBuilder.DropColumn(
                name: "ID_Ve",
                table: "DonHangChiTiets");

            migrationBuilder.AlterColumn<int>(
                name: "ID_HangGui",
                table: "DonHangChiTiets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangChiTiets_HangGuis_ID_HangGui",
                table: "DonHangChiTiets",
                column: "ID_HangGui",
                principalTable: "HangGuis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
