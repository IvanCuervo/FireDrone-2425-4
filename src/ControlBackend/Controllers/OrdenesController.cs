using Azure;
using ControlBackend.Interfaces;
using ControlBackend.Servicios;
using DroneController;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;



namespace ControlBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesController : ControllerBase
    {

        readonly IBroker _publicacionPlanVuelo;

        public OrdenesController(IBroker publicacionPlanVuelo)
        {
            _publicacionPlanVuelo = publicacionPlanVuelo;
        }

        [HttpPost("inicio")]
        public ActionResult IniciarPlanVuelo([FromBody] Conexion conexion)
        {
            DroneCommand dron = new DroneCommand()
            {
                Command = DroneCommand.START_FLIGHT_PLAN_CMD,
                Arguments = conexion.puntos,
            };

            _publicacionPlanVuelo.Publish("ColaDron" + conexion.dronId, dron.Codificar());

            return Ok();
        }

        [HttpPost("parar")]
        public ActionResult PararPlanVuelo()
        {

            DroneCommand dron = new DroneCommand()
            {
                Command = DroneCommand.STOP_FLIGHT_PLAN_CMD,
                Arguments = ""
            };

            _publicacionPlanVuelo.Publish("ColaDron" + 1, dron.Codificar());

            return Ok();
        }

        [HttpPost("estadodron/{id}")]
        public ActionResult EstadoDron(int id)
        {

            DroneCommand dron = new DroneCommand()
            {
                Command = DroneCommand.STATUS_CMD,
                Arguments = ""
            };

            _publicacionPlanVuelo.Publish("ColaDron" + id, dron.Codificar());

            return Ok();
        }
    }
}
