using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentralBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.AreaId);
                });

            migrationBuilder.CreateTable(
                name: "Rutas",
                columns: table => new
                {
                    RutaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Estado = table.Column<string>(type: "TEXT", nullable: true),
                    Riesgo = table.Column<string>(type: "TEXT", nullable: true),
                    Periodica = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroPeriodicidad = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutas", x => x.RutaId);
                });

            migrationBuilder.CreateTable(
                name: "EstacionesBase",
                columns: table => new
                {
                    EstacionBaseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstacionesBase", x => x.EstacionBaseId);
                    table.ForeignKey(
                        name: "FK_EstacionesBase_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstacionesControl",
                columns: table => new
                {
                    EstacionControlId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstacionesControl", x => x.EstacionControlId);
                    table.ForeignKey(
                        name: "FK_EstacionesControl_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreaRuta",
                columns: table => new
                {
                    AreasAreaId = table.Column<int>(type: "INTEGER", nullable: false),
                    RutasRutaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaRuta", x => new { x.AreasAreaId, x.RutasRutaId });
                    table.ForeignKey(
                        name: "FK_AreaRuta_Areas_AreasAreaId",
                        column: x => x.AreasAreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaRuta_Rutas_RutasRutaId",
                        column: x => x.RutasRutaId,
                        principalTable: "Rutas",
                        principalColumn: "RutaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PuntosRuta",
                columns: table => new
                {
                    PuntoRutaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    Secuencial = table.Column<int>(type: "INTEGER", nullable: false),
                    RutaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuntosRuta", x => x.PuntoRutaId);
                    table.ForeignKey(
                        name: "FK_PuntosRuta_Rutas_RutaId",
                        column: x => x.RutaId,
                        principalTable: "Rutas",
                        principalColumn: "RutaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drones",
                columns: table => new
                {
                    DronId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Modelo = table.Column<string>(type: "TEXT", nullable: true),
                    Camara = table.Column<string>(type: "TEXT", nullable: true),
                    Velocidad = table.Column<double>(type: "REAL", nullable: false),
                    AutonomiaVuelo = table.Column<double>(type: "REAL", nullable: false),
                    TiempoRecarga = table.Column<double>(type: "REAL", nullable: false),
                    Simulador = table.Column<string>(type: "TEXT", nullable: true),
                    Estado = table.Column<string>(type: "TEXT", nullable: true),
                    Sensores = table.Column<string>(type: "TEXT", nullable: true),
                    EstacionBaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    EstacionControlId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drones", x => x.DronId);
                    table.ForeignKey(
                        name: "FK_Drones_EstacionesBase_EstacionBaseId",
                        column: x => x.EstacionBaseId,
                        principalTable: "EstacionesBase",
                        principalColumn: "EstacionBaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Drones_EstacionesControl_EstacionControlId",
                        column: x => x.EstacionControlId,
                        principalTable: "EstacionesControl",
                        principalColumn: "EstacionControlId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanesVuelo",
                columns: table => new
                {
                    PlanVueloId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ControlManual = table.Column<int>(type: "INTEGER", nullable: false),
                    DronId = table.Column<int>(type: "INTEGER", nullable: false),
                    RutaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesVuelo", x => x.PlanVueloId);
                    table.ForeignKey(
                        name: "FK_PlanesVuelo_Drones_DronId",
                        column: x => x.DronId,
                        principalTable: "Drones",
                        principalColumn: "DronId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanesVuelo_Rutas_RutaId",
                        column: x => x.RutaId,
                        principalTable: "Rutas",
                        principalColumn: "RutaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicionesPlanVuelo",
                columns: table => new
                {
                    MedicionPlanVueloId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hora = table.Column<string>(type: "TEXT", nullable: true),
                    ImagenTermica = table.Column<string>(type: "TEXT", nullable: true),
                    ImagenNormal = table.Column<string>(type: "TEXT", nullable: true),
                    Humedad = table.Column<int>(type: "INTEGER", nullable: false),
                    Temperatura = table.Column<double>(type: "REAL", nullable: false),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    Altura = table.Column<double>(type: "REAL", nullable: false),
                    Velocidad = table.Column<double>(type: "REAL", nullable: false),
                    ModoDeVuelo = table.Column<string>(type: "TEXT", nullable: true),
                    SensoresActivados = table.Column<string>(type: "TEXT", nullable: true),
                    PlanVueloId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicionesPlanVuelo", x => x.MedicionPlanVueloId);
                    table.ForeignKey(
                        name: "FK_MedicionesPlanVuelo_PlanesVuelo_PlanVueloId",
                        column: x => x.PlanVueloId,
                        principalTable: "PlanesVuelo",
                        principalColumn: "PlanVueloId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PuntoPlanVuelo",
                columns: table => new
                {
                    PuntoPlanVueloId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    Secuencial = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanVueloId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuntoPlanVuelo", x => x.PuntoPlanVueloId);
                    table.ForeignKey(
                        name: "FK_PuntoPlanVuelo_PlanesVuelo_PlanVueloId",
                        column: x => x.PlanVueloId,
                        principalTable: "PlanesVuelo",
                        principalColumn: "PlanVueloId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incidencias",
                columns: table => new
                {
                    IncidenciaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Informacion = table.Column<string>(type: "TEXT", nullable: true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hora = table.Column<string>(type: "TEXT", nullable: true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    MedicionPlanVueloId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidencias", x => x.IncidenciaId);
                    table.ForeignKey(
                        name: "FK_Incidencias_MedicionesPlanVuelo_MedicionPlanVueloId",
                        column: x => x.MedicionPlanVueloId,
                        principalTable: "MedicionesPlanVuelo",
                        principalColumn: "MedicionPlanVueloId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaRuta_RutasRutaId",
                table: "AreaRuta",
                column: "RutasRutaId");

            migrationBuilder.CreateIndex(
                name: "IX_Drones_EstacionBaseId",
                table: "Drones",
                column: "EstacionBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Drones_EstacionControlId",
                table: "Drones",
                column: "EstacionControlId");

            migrationBuilder.CreateIndex(
                name: "IX_EstacionesBase_AreaId",
                table: "EstacionesBase",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstacionesControl_AreaId",
                table: "EstacionesControl",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_MedicionPlanVueloId",
                table: "Incidencias",
                column: "MedicionPlanVueloId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicionesPlanVuelo_PlanVueloId",
                table: "MedicionesPlanVuelo",
                column: "PlanVueloId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanesVuelo_DronId",
                table: "PlanesVuelo",
                column: "DronId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanesVuelo_RutaId",
                table: "PlanesVuelo",
                column: "RutaId");

            migrationBuilder.CreateIndex(
                name: "IX_PuntoPlanVuelo_PlanVueloId",
                table: "PuntoPlanVuelo",
                column: "PlanVueloId");

            migrationBuilder.CreateIndex(
                name: "IX_PuntosRuta_RutaId",
                table: "PuntosRuta",
                column: "RutaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaRuta");

            migrationBuilder.DropTable(
                name: "Incidencias");

            migrationBuilder.DropTable(
                name: "PuntoPlanVuelo");

            migrationBuilder.DropTable(
                name: "PuntosRuta");

            migrationBuilder.DropTable(
                name: "MedicionesPlanVuelo");

            migrationBuilder.DropTable(
                name: "PlanesVuelo");

            migrationBuilder.DropTable(
                name: "Drones");

            migrationBuilder.DropTable(
                name: "Rutas");

            migrationBuilder.DropTable(
                name: "EstacionesBase");

            migrationBuilder.DropTable(
                name: "EstacionesControl");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
