
namespace ControlBackend;

using System;
using ControlBackend.Broker;
using ControlBackend.Data;
using ControlBackend.Interfaces;
using ControlBackend.Servicios;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Agregar el contexto de la base de datos con SQLite
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=MyDatabase.db")); // Cambia el nombre de la base de datos seg�n lo desees

        // Para generar informaci�n de depuraci�n en caso de error
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        IBroker conexion = new RabbitMqBroker("localhost", "guest", "guest");
        builder.Services.AddSingleton<IBroker>(conexion);

        conexion.Subscribe("Estado.*", new ReceptorMensajes());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
