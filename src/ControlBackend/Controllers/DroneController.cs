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
        public ActionResult<IEnumerable<string>> GetDroneStatus([FromRoute] int id)
        {

            // Publicar el comando STATUS_CMD
            Log.Information("Status request for drone {0} received from CENTRALBACKEND", id);

            //Parseamos los datos
            DroneCommand dron = new DroneCommand()
            {

                Command = DroneCommand.STATUS_CMD,
                Arguments = ""
            };

            //Publicamos en la cola la peticion de datos del dron
            _pubsub_dron.Publish("ControlBackend.Controller." + id, dron.Encode());

            // Retorno de la respuesta
            return Ok("");
        }
    }
}
