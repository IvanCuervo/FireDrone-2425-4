namespace CentralFrontend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Construir la aplicaci�n
        var app = builder.Build();

        // Agregar middleware para servir archivos est�ticos
        app.UseStaticFiles();

        // Mapeo de una ruta simple opcional
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
