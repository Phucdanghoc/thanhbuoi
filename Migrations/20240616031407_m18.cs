using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Xes_loaiXes_ID_LoaiXe",
                table: "Xes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_loaiXes",
                table: "loaiXes");

            migrationBuilder.RenameTable(
                name: "loaiXes",
                newName: "LoaiXes");

            migrationBuilder.RenameIndex(
                name: "IX_loaiXes_Ten",
                table: "LoaiXes",
                newName: "IX_LoaiXes_Ten");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiXes",
                table: "LoaiXes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Xes_LoaiXes_ID_LoaiXe",
                table: "Xes",
                column: "ID_LoaiXe",
                principalTable: "LoaiXes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Xes_LoaiXes_ID_LoaiXe",
                table: "Xes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiXes",
                table: "LoaiXes");

            migrationBuilder.RenameTable(
                name: "LoaiXes",
                newName: "loaiXes");

            migrationBuilder.RenameIndex(
                name: "IX_LoaiXes_Ten",
                table: "loaiXes",
                newName: "IX_loaiXes_Ten");

            migrationBuilder.AddPrimaryKey(
                name: "PK_loaiXes",
                table: "loaiXes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Xes_loaiXes_ID_LoaiXe",
                table: "Xes",
                column: "ID_LoaiXe",
                principalTable: "loaiXes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
