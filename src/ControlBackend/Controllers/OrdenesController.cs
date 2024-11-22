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
        public ActionResult IniciarPlanVuelo()
        {
            var waypoints = new List<Waypoint>
{
                new Waypoint
                {
                    Latitude = 37.7749,
                    Longitude = -122.4194,
                    Altitude = 100.0,
                    Speed = 50.0
                },
                new Waypoint
                {
                    Latitude = 34.0522,
                    Longitude = -118.2437,
                    Altitude = 150.0,
                    Speed = 60.0
                },
                new Waypoint
                {
                    Latitude = 40.7128,
                    Longitude = -74.006,
                    Altitude = 200.0,
                    Speed = 70.0
                }
            };

            string waypointsJson = JsonConvert.SerializeObject(waypoints);



            DroneCommand dron = new DroneCommand()
            {
                Command = DroneCommand.START_FLIGHT_PLAN_CMD,
                Arguments = waypointsJson
            };

            _publicacionPlanVuelo.Publish("ColaDron" + 1, dron.Codificar());

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
