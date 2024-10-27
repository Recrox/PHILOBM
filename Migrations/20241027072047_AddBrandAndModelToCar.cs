using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PHILOBM.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandAndModelToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumeroPlaque",
                table: "Voitures",
                newName: "Model");

            migrationBuilder.RenameColumn(
                name: "NumeroChassis",
                table: "Voitures",
                newName: "LicensePlate");

            migrationBuilder.RenameColumn(
                name: "Kilometrage",
                table: "Voitures",
                newName: "Mileage");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Voitures",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChassisNumber",
                table: "Voitures",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Voitures");

            migrationBuilder.DropColumn(
                name: "ChassisNumber",
                table: "Voitures");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Voitures",
                newName: "NumeroPlaque");

            migrationBuilder.RenameColumn(
                name: "Mileage",
                table: "Voitures",
                newName: "Kilometrage");

            migrationBuilder.RenameColumn(
                name: "LicensePlate",
                table: "Voitures",
                newName: "NumeroChassis");
        }
    }
}
