 using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HangGuis_Chuyens_ID_Chuyen",
                table: "HangGuis");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "HangGuis",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayTao",
                table: "HangGuis",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Chuyen",
                table: "HangGuis",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HangGuis_Chuyens_ID_Chuyen",
                table: "HangGuis",
                column: "ID_Chuyen",
                principalTable: "Chuyens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HangGuis_Chuyens_ID_Chuyen",
                table: "HangGuis");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "HangGuis",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayTao",
                table: "HangGuis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Chuyen",
                table: "HangGuis",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HangGuis_Chuyens_ID_Chuyen",
                table: "HangGuis",
                column: "ID_Chuyen",
                principalTable: "Chuyens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
