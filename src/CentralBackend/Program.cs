namespace CentralBackend;
using CentralBackend.Data;
using CentralBackend.Hubs;
using Microsoft.EntityFrameworkCore;
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
        options.UseSqlite("Data Source=../../FireDrone-2425.db"));


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
}