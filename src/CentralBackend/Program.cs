namespace CentralBackend;
using CentralBackend.Data;
using CentralBackend.Hubs;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // En la configuración de los servicios:
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.MaxDepth = 64; // O ajusta según lo necesario
            });

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite("Data Source=../../FireDrone-2425-4.db"));

        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated(); // Crea la base de datos si no existe
            SeedData(dbContext); //Añade datos a la base de datos
        }

        // Opcional: para ver errores de base de datos en desarrollo
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                policy.WithOrigins("http://localhost:5035")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials(); // Permitir credenciales si se necesitan
            });
        });

        // Agregar servicios de SignalR
        builder.Services.AddSignalR();

        var app = builder.Build();

        app.UseCors(builder => builder
       .SetIsOriginAllowed((host) => true)
       .AllowAnyMethod()
       .AllowAnyHeader()
       .AllowCredentials()
       );

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        // Mapeo de SignalR
        app.MapHub<UpdateMapHub>("/updatemaphub");

        // Mapeo del SignalR
        app.MapHub<UpdatePlanesVueloHub>("/updateplanesvuelohub");

        app.Run();
    }


    static void SeedData(AppDbContext context)
    {
        if (!context.Areas.Any())
        {
            context.Areas.AddRange(
                new Area { AreaId = 1, X = 39.46, Y = -6.37 },
                new Area { AreaId = 2, X = 40.32, Y = -3.70 },
                new Area { AreaId = 3, X = 38.90, Y = -4.25 },
                new Area { AreaId = 4, X = 41.23, Y = -5.87 },
                new Area { AreaId = 5, X = 39.20, Y = -3.35 }
            );
        }

        if (!context.EstacionesBase.Any())
        {
            context.EstacionesBase.AddRange(
                new EstacionBase { EstacionBaseId = 1, X = 39.47, Y = -6.38, AreaId = 1 },
                new EstacionBase { EstacionBaseId = 2, X = 40.33, Y = -3.71, AreaId = 2 },
                new EstacionBase { EstacionBaseId = 3, X = 38.91, Y = -4.26, AreaId = 3 },
                new EstacionBase { EstacionBaseId = 4, X = 41.24, Y = -5.88, AreaId = 4 },
                new EstacionBase { EstacionBaseId = 5, X = 39.21, Y = -3.36, AreaId = 5 }
            );
        }

        if (!context.EstacionesControl.Any())
        {
            context.EstacionesControl.AddRange(
                new EstacionControl { EstacionControlId = 1, X = 39.48, Y = -6.39, AreaId = 1 },
                new EstacionControl { EstacionControlId = 2, X = 40.34, Y = -3.72, AreaId = 2 },
                new EstacionControl { EstacionControlId = 3, X = 38.92, Y = -4.27, AreaId = 3 },
                new EstacionControl { EstacionControlId = 4, X = 41.25, Y = -5.89, AreaId = 4 },
                new EstacionControl { EstacionControlId = 5, X = 39.22, Y = -3.37, AreaId = 5 }
            );
        }

        if (!context.Drones.Any())
        {
            context.Drones.AddRange(
                new Dron { DronId = 1, Modelo = "DJI Mavic Pro", Camara = "4K", Velocidad = 50.5, AutonomiaVuelo = 30.0, TiempoRecarga = 1.5, Simulador = "Simulador A", Estado = "Operativo", Sensores = "Infrarrojo", Bateria = 89.4, EstacionBaseId = 1, EstacionControlId = 1 },
                new Dron { DronId = 2, Modelo = "Parrot Anafi", Camara = "HD", Velocidad = 45.0, AutonomiaVuelo = 25.0, TiempoRecarga = 1.0, Simulador = "Simulador B", Estado = "En mantenimiento", Sensores = "Ultrasonido", Bateria = 79.4, EstacionBaseId = 2, EstacionControlId = 2 },
                new Dron { DronId = 3, Modelo = "Autel Evo", Camara = "4K", Velocidad = 55.5, AutonomiaVuelo = 35.0, TiempoRecarga = 2.0, Simulador = "Simulador C", Estado = "Operativo", Sensores = "Infrarrojo y ultrasonido", Bateria = 39.4, EstacionBaseId = 3, EstacionControlId = 3 },
                new Dron { DronId = 4, Modelo = "DJI Phantom 4", Camara = "HD", Velocidad = 60.0, AutonomiaVuelo = 40.0, TiempoRecarga = 1.8, Simulador = "Simulador A", Estado = "Operativo", Sensores = "Termografía", Bateria = 58.4, EstacionBaseId = 4, EstacionControlId = 4 },
                new Dron { DronId = 5, Modelo = "Yuneec Typhoon", Camara = "4K", Velocidad = 52.0, AutonomiaVuelo = 28.0, TiempoRecarga = 1.2, Simulador = "Simulador B", Estado = "Operativo", Sensores = "Infrarrojo y termografía", Bateria = 65.4, EstacionBaseId = 5, EstacionControlId = 5 }
            );
        }

        if (!context.Rutas.Any())
        {
            context.Rutas.AddRange(
                new Ruta { RutaId = 1, Estado = "Activa", Riesgo = "Alto", Periodica = "true", NumeroPeriodicidad = 15 },
                new Ruta { RutaId = 2, Estado = "Inactiva", Riesgo = "Medio", Periodica = "false", NumeroPeriodicidad = 0 },
                new Ruta { RutaId = 3, Estado = "Activa", Riesgo = "Bajo", Periodica = "true", NumeroPeriodicidad = 30 },
                new Ruta { RutaId = 4, Estado = "En revisión", Riesgo = "Alto", Periodica = "true", NumeroPeriodicidad = 7 },
                new Ruta { RutaId = 5, Estado = "Activa", Riesgo = "Medio", Periodica = "false", NumeroPeriodicidad = 0 }
            );
        }

        if (!context.PlanesVuelo.Any())
        {
            context.PlanesVuelo.AddRange(
                new PlanVuelo { PlanVueloId = 1, FechaInicio = "2024-11-01", FechaFin = "2024-11-05", ControlManual = 0, DronId = 1, RutaId = 1 },
                new PlanVuelo { PlanVueloId = 2, FechaInicio = "2024-11-06", FechaFin = "2024-11-10", ControlManual = 0, DronId = 2, RutaId = 2 },
                new PlanVuelo { PlanVueloId = 3, FechaInicio = "2024-11-11", FechaFin = "2024-11-15", ControlManual = 0, DronId = 3, RutaId = 3 },
                new PlanVuelo { PlanVueloId = 4, FechaInicio = "2024-11-16", FechaFin = "2024-11-20", ControlManual = 0, DronId = 4, RutaId = 4 },
                new PlanVuelo { PlanVueloId = 5, FechaInicio = "2024-11-21", FechaFin = "2024-11-25", ControlManual = 0, DronId = 5, RutaId = 5 }
            );
        }

        if (!context.PuntosPlanVuelo.Any())
        {
            context.PuntosPlanVuelo.AddRange(
                new PuntoPlanVuelo { PuntoPlanVueloId = 1, X = 39.47, Y = -6.38, Secuencial = 1, Latitud = 39.4701, Altitud = 500.0, PlanVueloId = 1 },
                new PuntoPlanVuelo { PuntoPlanVueloId = 2, X = 39.47, Y = -6.39, Secuencial = 2, Latitud = 39.4702, Altitud = 510.0, PlanVueloId = 1 },
                new PuntoPlanVuelo { PuntoPlanVueloId = 3, X = 39.48, Y = -6.38, Secuencial = 1, Latitud = 39.4801, Altitud = 520.0, PlanVueloId = 2 },
                new PuntoPlanVuelo { PuntoPlanVueloId = 4, X = 39.48, Y = -6.39, Secuencial = 2, Latitud = 39.4802, Altitud = 530.0, PlanVueloId = 2 },
                new PuntoPlanVuelo { PuntoPlanVueloId = 5, X = 39.49, Y = -6.38, Secuencial = 3, Latitud = 39.4901, Altitud = 540.0, PlanVueloId = 3 }
            );
        }

        if (!context.PuntosRuta.Any())
        {
            context.PuntosRuta.AddRange(
                new PuntoRuta { PuntoRutaId = 1, X = 39.48, Y = -6.38, Secuencial = 1, Latitud = 39.4801, Altitud = 450.0, RutaId = 1 },
                new PuntoRuta { PuntoRutaId = 2, X = 39.49, Y = -6.39, Secuencial = 2, Latitud = 39.4901, Altitud = 460.0, RutaId = 1 },
                new PuntoRuta { PuntoRutaId = 3, X = 40.32, Y = -3.70, Secuencial = 1, Latitud = 40.3201, Altitud = 470.0, RutaId = 2 },
                new PuntoRuta { PuntoRutaId = 4, X = 40.33, Y = -3.71, Secuencial = 2, Latitud = 40.3301, Altitud = 480.0, RutaId = 2 },
                new PuntoRuta { PuntoRutaId = 5, X = 38.90, Y = -4.25, Secuencial = 1, Latitud = 38.9001, Altitud = 490.0, RutaId = 3 }
            );
        }

        context.SaveChanges();
    }

}