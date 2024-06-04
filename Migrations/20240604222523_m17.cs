using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GheId",
                table: "VeHuys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VeHuys_GheId",
                table: "VeHuys",
                column: "GheId");

            migrationBuilder.AddForeignKey(
                name: "FK_VeHuys_Ghes_GheId",
                table: "VeHuys",
                column: "GheId",
                principalTable: "Ghes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VeHuys_Ghes_GheId",
                table: "VeHuys");

            migrationBuilder.DropIndex(
                name: "IX_VeHuys_GheId",
                table: "VeHuys");

            migrationBuilder.DropColumn(
                name: "GheId",
                table: "VeHuys");
        }
    }
}
