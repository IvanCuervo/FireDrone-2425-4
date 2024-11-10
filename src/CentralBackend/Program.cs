
namespace CentralBackend;
using CentralBackend.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite("Data Source=src\\CentralBackend\\Library.db"));


        builder.Services.AddCors();


        // Opcional: para ver errores de base de datos en desarrollo
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();


        // En la configuración de los servicios:
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.MaxDepth = 64; // O ajusta según lo necesario
            });


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

        app.Run();
    }
}
