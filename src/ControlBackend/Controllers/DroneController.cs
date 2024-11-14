using Serilog;
using DroneController;
using Microsoft.AspNetCore.Mvc;

namespace ControlBackend.Controllers
{
    [ApiController]
    [Route("ControlBackend/[controller]")]
    public class DroneController : ControllerBase
    {
        readonly IBroker _pubsub_dron;

        public DroneController(IBroker pubsub)
        {
            _pubsub_dron = pubsub;
        }

        [HttpGet("status/{id}")]
        public ActionResult<DroneResponse> GetDroneStatus([FromRoute] int id)
        {
            // Validar el ID
            if (id <= 0)
            {
                Log.Warning("Invalid drone ID {0} received", id);
                return BadRequest("Invalid drone ID");
            }

            try
            {
                Log.Information("Status request for drone {0} received from CENTRALBACKEND", id);

                // Construir el comando
                DroneCommand dron = BuildDroneStatusCommand();

                // Publicar el comando
                string topic = $"ControlBackend.Controller.{id}";
                _pubsub_dron.Publish(topic, dron.Encode());
                Log.Information("Status command for drone {0} published successfully", id);

                // Retornar respuesta significativa
                return Ok(new DroneResponse
                {
                    Message = $"Status command for drone {id} has been published.",
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to publish status command for drone {0}", id);
                return StatusCode(500, "Failed to process the request");
            }
        }

        private DroneCommand BuildDroneStatusCommand()
        {
            return new DroneCommand
            {
                Command = DroneCommand.STATUS_CMD,
                Arguments = ""
            };
        }
    }

    public class DroneResponse
    {
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
