using Microsoft.AspNetCore.Mvc;
using ControlBackend.Interfaces;
using System.Text;
using DroneController;

namespace ControlBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DroneController : ControllerBase
    {
        readonly IBroker _publicacion_dron;

        public DroneController(IBroker publicacion_dron)
        {
            _publicacion_dron = publicacion_dron;
        }


        [HttpGet("estado/{id}")]
        public ActionResult<IEnumerable<string>> GetDroneStatus([FromRoute] int id)
        {
            DroneCommand dron = new DroneCommand()
            {

                Command = DroneCommand.STATUS_CMD,
                Arguments = ""
            };

            //Publicamos en la cola la peticion de datos del dron
            _publicacion_dron.Publish("ColaDron" + id, dron.Codificar());

            // Retorno de la respuesta
            return Ok();
        }
    }
}
