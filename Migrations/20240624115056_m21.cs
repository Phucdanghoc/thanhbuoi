using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrongTaiHang",
                table: "Xes");

            migrationBuilder.DropColumn(
                name: "isCancel",
                table: "Ves");

            migrationBuilder.DropColumn(
                name: "TrongLuong",
                table: "HangGuis");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Ves",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "DonHangs",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "DiaChiNguoiGui",
                table: "HangGuis",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianDen",
                table: "Chuyens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaChiNguoiGui",
                table: "HangGuis");

            migrationBuilder.DropColumn(
                name: "ThoiGianDen",
                table: "Chuyens");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Ves",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "DonHangs",
                newName: "email");

            migrationBuilder.AddColumn<double>(
                name: "TrongTaiHang",
                table: "Xes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "isCancel",
                table: "Ves",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "TrongLuong",
                table: "HangGuis",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
