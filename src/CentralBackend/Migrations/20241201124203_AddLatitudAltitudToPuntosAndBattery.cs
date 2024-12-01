using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentralBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddLatitudAltitudToPuntosAndBattery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Altitud",
                table: "PuntosRuta",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "PuntosRuta",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Bateria",
                table: "Drones",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Altitud",
                table: "PuntosPlanVuelo",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "PuntosPlanVuelo",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altitud",
                table: "PuntosRuta");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "PuntosRuta");

            migrationBuilder.DropColumn(
                name: "Bateria",
                table: "Drones");

            migrationBuilder.DropColumn(
                name: "Altitud",
                table: "PuntosPlanVuelo");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "PuntosPlanVuelo");
        }
    }
}
