using Newtonsoft.Json;
using System.Text;

namespace DroneController
{
    public class DroneCommand
    {
        public const string START_FLIGHT_PLAN_CMD = "StartFlightPlan";
        public const string STOP_FLIGHT_PLAN_CMD = "StopFlightPlan";
        public const string STATUS_CMD = "Status";

        public string? Command { get; set; }
        public string? Arguments { get; set; }

        public Byte[] Codificar()
        {
            var mensaje = "";
            byte[] mensajeCodificado = [];
            try
            {
                 mensaje = JsonConvert.SerializeObject(this);
                 mensajeCodificado = Encoding.UTF8.GetBytes(mensaje);
            } catch (JsonException ex) {

                Console.WriteLine($"Error al deserializar los argumentos: {ex.Message}");
            }
            return mensajeCodificado;
        }
    }
}