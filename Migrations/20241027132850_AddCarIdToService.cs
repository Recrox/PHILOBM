using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PHILOBM.Migrations
{
    /// <inheritdoc />
    public partial class AddCarIdToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Services",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_CarId",
                table: "Services",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Cars_CarId",
                table: "Services",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Cars_CarId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_CarId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Services");
        }
    }
}
