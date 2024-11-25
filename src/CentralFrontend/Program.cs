namespace CentralFrontend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Construir la aplicación
        var app = builder.Build();

        // Agregar middleware para servir archivos estáticos
        app.UseStaticFiles();

        // Mapeo de una ruta simple opcional
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
