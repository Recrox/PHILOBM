using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PHILOBM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "Clients",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Prenom",
                table: "Clients",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Clients",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Adresse",
                table: "Clients",
                newName: "Adress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Clients",
                newName: "Telephone");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Clients",
                newName: "Prenom");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Clients",
                newName: "Nom");

            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Clients",
                newName: "Adresse");
        }
    }
}
