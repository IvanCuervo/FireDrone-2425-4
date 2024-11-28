using CentralBackend.Data;
using CentralBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net.Http.Headers;

namespace CentralBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoDronController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly EstadoDronService _service;

        public EstadoDronController(AppDbContext context)
        {
            _context = context;

            HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5057/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            _service = new EstadoDronService(_client);
        }

        [HttpPost("estado/{id}")]
        public ActionResult EstadoDron(int id)
        {
            _service.EstadoDron(id);

            return Ok();
        }

        [HttpPost("recibirestado")]
        public async Task<IActionResult> RecibirEstadoDron([FromBody] MedicionPlanVuelo medicion)
        {
            // Validar que la medición no sea nula
            if (medicion == null)
            {
                return BadRequest("La medición enviada es nula.");
            }

            Console.WriteLine($"Recibida medición con ID: {medicion.MedicionPlanVueloId}");

            //// Obtener el PlanVuelo asociado al MedicionPlanVuelo
            //var planVuelo = _context.PlanesVuelo.FirstOrDefault(p => p.PlanVueloId == medicion.PlanVuelo.PlanVueloId);

            //if (planVuelo == null)
            //{
            //    return NotFound($"No se encontró el PlanVuelo con ID {medicion.PlanVueloId}");
            //}

            //// Obtener el DronId desde el PlanVuelo
            //int dronId = planVuelo.DronId;

            //Console.WriteLine($"Dron asociado al PlanVuelo (ID: {planVuelo.PlanVueloId}): DronId = {dronId}");

            // Crear un objeto DroneInfo con los datos necesarios
            var dronInfo = new DroneInfo
            {
                DronId = medicion.PlanVuelo.DronId,             // Incluir el DronId en el objeto DroneInfo
                Latitude = medicion.X,      // Interpretamos X como la latitud
                Longitude = medicion.Y,     // Interpretamos Y como la longitud
                Altitude = medicion.Altura, // Altura desde MedicionPlanVuelo
                Speed = medicion.Velocidad, // Velocidad desde MedicionPlanVuelo
                Battery = 75.0             // Valor ficticio o cálculo de batería
            };

            // Llamar a ActualizarPosicionDron para enviar la información actualizada
            await _service.ActualizarPosicionDron(dronInfo);

            //_context.MedicionesPlanVuelo.Add(medicion);
            //_context.SaveChanges();

            return Ok("Medición procesada correctamente.");
        }

    }
}