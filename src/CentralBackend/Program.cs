
namespace CentralBackend;
using CentralBackend.Data;
using CentralBackend.Hubs;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>(options =>
              options.UseSqlite("Data Source=FireDrone-2425-4.db"));

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

        app.UseCors("AllowSpecificOrigin");

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

        //var summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        //app.MapGet("/weatherforecast", (HttpContext httpContext) =>
        //{
        //    var forecast =  Enumerable.Range(1, 5).Select(index =>
        //        new WeatherForecast
        //        {
        //            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //            TemperatureC = Random.Shared.Next(-20, 55),
        //            Summary = summaries[Random.Shared.Next(summaries.Length)]
        //        })
        //        .ToArray();
        //    return forecast;
        //})
        //.WithName("GetWeatherForecast")
        //.WithOpenApi();

        app.Run();
    }
}
