using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VeHuys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CMND = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_Chuyen = table.Column<int>(type: "int", nullable: false),
                    hoantien = table.Column<double>(type: "float", nullable: false),
                    ngaytao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeHuys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VeHuys_Chuyens_ID_Chuyen",
                        column: x => x.ID_Chuyen,
                        principalTable: "Chuyens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VeHuys_ID_Chuyen",
                table: "VeHuys",
                column: "ID_Chuyen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VeHuys");
        }
    }
}
