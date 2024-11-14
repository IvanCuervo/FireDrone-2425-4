using Serilog;
using Microsoft.AspNetCore.Mvc;

namespace ControlBackend.Controllers
{
    [ApiController]
    [Route("ControlBackend/[controller]")]
    public class DroneController : ControllerBase
    {
        [HttpGet("status/{id}")]
        public ActionResult<string> GetDroneStatus([FromRoute] int id)
        {
            // Log de la solicitud de estado
            Log.Information("Status request for drone {0} received from CENTRALBACKEND", id);

            // Crear y codificar el comando STATUS_CMD
            var droneCommand = new
            {
                Command = "STATUS_CMD",
                Arguments = ""
            };
            var encodedCommand = System.Text.Json.JsonSerializer.Serialize(droneCommand);

            // Simular publicación en la cola
            Log.Information("Published command to topic: {0}", $"ControlBackend.Controller.{id}");
            Log.Information("Command: {0}", encodedCommand);

            // Retorno de la respuesta
            return Ok("Command published successfully.");
        }
    }
}
