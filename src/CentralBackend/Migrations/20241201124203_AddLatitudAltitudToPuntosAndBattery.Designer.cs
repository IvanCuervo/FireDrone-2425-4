﻿// <auto-generated />
using CentralBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CentralBackend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241201124203_AddLatitudAltitudToPuntosAndBattery")]
    partial class AddLatitudAltitudToPuntosAndBattery
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("AreaRuta", b =>
                {
                    b.Property<int>("AreasAreaId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RutasRutaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AreasAreaId", "RutasRutaId");

                    b.HasIndex("RutasRutaId");

                    b.ToTable("AreaRuta");
                });

            modelBuilder.Entity("Models.Area", b =>
                {
                    b.Property<int>("AreaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("AreaId");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("Models.Dron", b =>
                {
                    b.Property<int>("DronId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("AutonomiaVuelo")
                        .HasColumnType("REAL");

                    b.Property<double>("Bateria")
                        .HasColumnType("REAL");

                    b.Property<string>("Camara")
                        .HasColumnType("TEXT");

                    b.Property<int>("EstacionBaseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EstacionControlId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Estado")
                        .HasColumnType("TEXT");

                    b.Property<string>("Modelo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Sensores")
                        .HasColumnType("TEXT");

                    b.Property<string>("Simulador")
                        .HasColumnType("TEXT");

                    b.Property<double>("TiempoRecarga")
                        .HasColumnType("REAL");

                    b.Property<double>("Velocidad")
                        .HasColumnType("REAL");

                    b.HasKey("DronId");

                    b.HasIndex("EstacionBaseId");

                    b.HasIndex("EstacionControlId");

                    b.ToTable("Drones");
                });

            modelBuilder.Entity("Models.EstacionBase", b =>
                {
                    b.Property<int>("EstacionBaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AreaId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("EstacionBaseId");

                    b.HasIndex("AreaId");

                    b.ToTable("EstacionesBase");
                });

            modelBuilder.Entity("Models.EstacionControl", b =>
                {
                    b.Property<int>("EstacionControlId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AreaId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("EstacionControlId");

                    b.HasIndex("AreaId");

                    b.ToTable("EstacionesControl");
                });

            modelBuilder.Entity("Models.Incidencia", b =>
                {
                    b.Property<int>("IncidenciaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<string>("Informacion")
                        .HasColumnType("TEXT");

                    b.Property<int>("MedicionPlanVueloId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("IncidenciaId");

                    b.HasIndex("MedicionPlanVueloId");

                    b.ToTable("Incidencias");
                });

            modelBuilder.Entity("Models.MedicionPlanVuelo", b =>
                {
                    b.Property<int>("MedicionPlanVueloId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Altura")
                        .HasColumnType("REAL");

                    b.Property<string>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<int>("Humedad")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImagenNormal")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImagenTermica")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModoDeVuelo")
                        .HasColumnType("TEXT");

                    b.Property<int>("PlanVueloId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SensoresActivados")
                        .HasColumnType("TEXT");

                    b.Property<double>("Temperatura")
                        .HasColumnType("REAL");

                    b.Property<double>("Velocidad")
                        .HasColumnType("REAL");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("MedicionPlanVueloId");

                    b.HasIndex("PlanVueloId");

                    b.ToTable("MedicionesPlanVuelo");
                });

            modelBuilder.Entity("Models.PlanVuelo", b =>
                {
                    b.Property<int>("PlanVueloId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ControlManual")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DronId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FechaFin")
                        .HasColumnType("TEXT");

                    b.Property<string>("FechaInicio")
                        .HasColumnType("TEXT");

                    b.Property<int>("RutaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlanVueloId");

                    b.HasIndex("DronId");

                    b.HasIndex("RutaId");

                    b.ToTable("PlanesVuelo");
                });

            modelBuilder.Entity("Models.PuntoPlanVuelo", b =>
                {
                    b.Property<int>("PuntoPlanVueloId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Altitud")
                        .HasColumnType("REAL");

                    b.Property<double>("Latitud")
                        .HasColumnType("REAL");

                    b.Property<int>("PlanVueloId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Secuencial")
                        .HasColumnType("INTEGER");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("PuntoPlanVueloId");

                    b.HasIndex("PlanVueloId");

                    b.ToTable("PuntosPlanVuelo");
                });

            modelBuilder.Entity("Models.PuntoRuta", b =>
                {
                    b.Property<int>("PuntoRutaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Altitud")
                        .HasColumnType("REAL");

                    b.Property<double>("Latitud")
                        .HasColumnType("REAL");

                    b.Property<int>("RutaId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Secuencial")
                        .HasColumnType("INTEGER");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("PuntoRutaId");

                    b.HasIndex("RutaId");

                    b.ToTable("PuntosRuta");
                });

            modelBuilder.Entity("Models.Ruta", b =>
                {
                    b.Property<int>("RutaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Estado")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumeroPeriodicidad")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Periodica")
                        .HasColumnType("TEXT");

                    b.Property<string>("Riesgo")
                        .HasColumnType("TEXT");

                    b.HasKey("RutaId");

                    b.ToTable("Rutas");
                });

            modelBuilder.Entity("AreaRuta", b =>
                {
                    b.HasOne("Models.Area", null)
                        .WithMany()
                        .HasForeignKey("AreasAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Ruta", null)
                        .WithMany()
                        .HasForeignKey("RutasRutaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Dron", b =>
                {
                    b.HasOne("Models.EstacionBase", "EstacionBase")
                        .WithMany("Drones")
                        .HasForeignKey("EstacionBaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.EstacionControl", "EstacionControl")
                        .WithMany("Drones")
                        .HasForeignKey("EstacionControlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EstacionBase");

                    b.Navigation("EstacionControl");
                });

            modelBuilder.Entity("Models.EstacionBase", b =>
                {
                    b.HasOne("Models.Area", "Area")
                        .WithMany("EstacionesBase")
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Area");
                });

            modelBuilder.Entity("Models.EstacionControl", b =>
                {
                    b.HasOne("Models.Area", "Area")
                        .WithMany("EstacionesControl")
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Area");
                });

            modelBuilder.Entity("Models.Incidencia", b =>
                {
                    b.HasOne("Models.MedicionPlanVuelo", "MedicionPlanVuelo")
                        .WithMany("Incidencias")
                        .HasForeignKey("MedicionPlanVueloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicionPlanVuelo");
                });

            modelBuilder.Entity("Models.MedicionPlanVuelo", b =>
                {
                    b.HasOne("Models.PlanVuelo", "PlanVuelo")
                        .WithMany("MedicionesPlanVuelo")
                        .HasForeignKey("PlanVueloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlanVuelo");
                });

            modelBuilder.Entity("Models.PlanVuelo", b =>
                {
                    b.HasOne("Models.Dron", "Dron")
                        .WithMany("PlanesVuelo")
                        .HasForeignKey("DronId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Ruta", "Ruta")
                        .WithMany("PlanesVuelo")
                        .HasForeignKey("RutaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dron");

                    b.Navigation("Ruta");
                });

            modelBuilder.Entity("Models.PuntoPlanVuelo", b =>
                {
                    b.HasOne("Models.PlanVuelo", "PlanVuelo")
                        .WithMany("PuntosPlanVuelo")
                        .HasForeignKey("PlanVueloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlanVuelo");
                });

            modelBuilder.Entity("Models.PuntoRuta", b =>
                {
                    b.HasOne("Models.Ruta", "Ruta")
                        .WithMany("PuntosRuta")
                        .HasForeignKey("RutaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ruta");
                });

            modelBuilder.Entity("Models.Area", b =>
                {
                    b.Navigation("EstacionesBase");

                    b.Navigation("EstacionesControl");
                });

            modelBuilder.Entity("Models.Dron", b =>
                {
                    b.Navigation("PlanesVuelo");
                });

            modelBuilder.Entity("Models.EstacionBase", b =>
                {
                    b.Navigation("Drones");
                });

            modelBuilder.Entity("Models.EstacionControl", b =>
                {
                    b.Navigation("Drones");
                });

            modelBuilder.Entity("Models.MedicionPlanVuelo", b =>
                {
                    b.Navigation("Incidencias");
                });

            modelBuilder.Entity("Models.PlanVuelo", b =>
                {
                    b.Navigation("MedicionesPlanVuelo");

                    b.Navigation("PuntosPlanVuelo");
                });

            modelBuilder.Entity("Models.Ruta", b =>
                {
                    b.Navigation("PlanesVuelo");

                    b.Navigation("PuntosRuta");
                });
#pragma warning restore 612, 618
        }
    }
}
