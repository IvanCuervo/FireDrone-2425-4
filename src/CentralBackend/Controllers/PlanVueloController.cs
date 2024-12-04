using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CentralBackend.Data;
using Models;
using CentralBackend.DTOs;
using Humanizer;
using CentralBackend.Services;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Routing;
using DroneController;

namespace CentralBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanVueloController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PlanVueloService _service;

        public PlanVueloController(AppDbContext context)
        {
            _context = context;

            HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5402/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            _service = new PlanVueloService(_client);
        }

        // GET: api/PlanVuelo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanVuelo>>> GetPlanesVuelo()
        {
            
            var planesVuelo = await _context.PlanesVuelo.ToListAsync();
            return Ok(planesVuelo);
        }

        // GET: api/PlanVuelo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlanVuelo>> GetPlanVuelo(int id)
        {
            var planVuelo = await _context.PlanesVuelo.FindAsync(id);

            if (planVuelo == null)
            {
                return NotFound();
            }

            return planVuelo;
        }

        // PUT: api/PlanVuelo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlanVuelo(int id, PlanVuelo planVuelo)
        {
            if (id != planVuelo.PlanVueloId)
            {
                return BadRequest();
            }

            _context.Entry(planVuelo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanVueloExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PlanVuelo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlanVuelo>> PostPlanVuelo([FromBody] PlanVueloDTO planVueloDto)
        {

            // Verificar si el Dron y la Ruta existen en la base de datos
            var dron = await _context.Drones.FindAsync(planVueloDto.DronId);
            if (dron == null)
            {
                return BadRequest("Dron no encontrado.");
            }

            var ruta = await _context.Rutas.FindAsync(planVueloDto.RutaId);
            if (ruta == null)
            {
                return BadRequest("Ruta no encontrada.");
            }

            // Creacion del Plan de Vuelo
            var planVuelo = new PlanVuelo
            {
                FechaInicio = planVueloDto.FechaInicio,
                FechaFin = planVueloDto.FechaFin,
                ControlManual = planVueloDto.ControlManual,
                Dron = dron,
                Ruta = ruta
            };

            _context.PlanesVuelo.Add(planVuelo);
            var savedChanges = await _context.SaveChangesAsync();

            // Comprobar si se guardo el plan de vuelo en la base de datos
            if (savedChanges > 0)
            {
                // Anadir puntos
                var puntosRuta = await _context.PuntosRuta
                    .Where(p => p.RutaId == planVueloDto.RutaId) // Filtrar por el campo RutaId
                    .OrderBy(p => p.Secuencial)                  // Ordenar por el campo Secuencia
                    .ToListAsync();
                // Convertir a PuntosPlanVuelo
                var puntosPlanVuelo = puntosRuta.Select(p => new PuntoPlanVuelo
                {
                    X = p.X,
                    Y = p.Y,
                    Secuencial = p.Secuencial,
                    PlanVueloId = planVuelo.PlanVueloId,
                    Latitud = p.Latitud, // CAMBIAR
                    Altitud = p.Altitud, // CAMBIAR
                }).ToList();
                if (ruta.Periodica == "true")
                {
                    // Obtener el último secuencial de la lista de puntos
                    int ultimoSecuencial = puntosPlanVuelo.Max(p => p.Secuencial);
                    // Crear una lista para almacenar los puntos duplicados
                    var puntosDuplicados = new List<PuntoPlanVuelo>();
                    for (int ronda = 1; ronda < ruta.NumeroPeriodicidad; ronda++)
                    {
                        foreach (var punto in puntosPlanVuelo)
                        {
                            // Crear una copia del punto original con un nuevo secuencial
                            var nuevoPunto = new PuntoPlanVuelo
                            {
                                X = punto.X,
                                Y = punto.Y,
                                Secuencial = ronda * ultimoSecuencial + punto.Secuencial,
                                PlanVueloId = punto.PlanVueloId,
                                Latitud = punto.Latitud, // CAMBIAR
                                Altitud = punto.Altitud, // CAMBIAR
                            };

                            // Agregar el nuevo punto a la lista de duplicados
                            puntosDuplicados.Add(nuevoPunto);
                        }
                    }
                    // Agregar los puntos duplicados a la lista original
                    puntosPlanVuelo.AddRange(puntosDuplicados);
                }
                    // Anadir estación base como ultimo punto
                    var estBase = await _context.EstacionesBase.FindAsync(dron.EstacionBaseId);
                if (estBase != null)
                {
                    puntosPlanVuelo.Add(new PuntoPlanVuelo
                    {
                        X = estBase.X,
                        Y = estBase.Y,
                        Secuencial = puntosPlanVuelo.Count + 1,
                        PlanVueloId = planVuelo.PlanVueloId,
                        Latitud = 50, // CAMBIAR
                        Altitud = 50, // CAMBIAR
                    });
                    foreach (var punto in puntosPlanVuelo)
                    {
                        _context.PuntosPlanVuelo.Add(punto); // Agregar cada punto a la bd
                    }
                    savedChanges = await _context.SaveChangesAsync();
                    if (savedChanges > 0)
                    {
                        // Puntos del plan de vuelo en orden
                        var puntosPlan = await _context.PuntosPlanVuelo
                            .Where(p => p.PlanVueloId == planVuelo.PlanVueloId) // Filtrar por el campo planVueloId
                            .OrderBy(p => p.Secuencial)                  // Ordenar por el campo Secuencia
                            .Select(p => new Waypoint { Longitude = p.X, Latitude = p.Y, Speed = p.Latitud, Altitude = p.Altitud }) // CAMBIAR
                            .ToListAsync();
                        var puntosJson = JsonConvert.SerializeObject(puntosPlan);
                        // Llamada servicio
                        Conexion conex = new Conexion()
                        {
                            dronId = dron.DronId,
                            driver = "DroneSimulator",
                            puntos = puntosJson,
                        };
                        _service.CrearPlanVuelo(conex);
                        return CreatedAtAction(nameof(GetPlanVuelo), new { id = planVuelo.PlanVueloId }, planVuelo);
                    }
                    else
                    {
                        return StatusCode(500, "Error al guardar los puntos del plan de vuelo.");
                    }
                }
                else
                {
                    return StatusCode(500, "No se ha encontrado la estación base.");
                }
            }
            else
            {
                return StatusCode(500, "Error al guardar el plan de vuelo.");
            }

        }

        [HttpPost("parar/{id}")]
        public async Task<ActionResult<PlanVuelo>> PararPlanVuelo(int id)
        {

            Conexion conex = new Conexion()
            {
                dronId = id,
                driver = "DroneSimulator",
                puntos = "",
            };

            _service.PararVuelo(conex);

            return Ok();
        }


        // DELETE: api/PlanVuelo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlanVuelo(int id)
        {
            var planVuelo = await _context.PlanesVuelo.FindAsync(id);
            if (planVuelo == null)
            {
                return NotFound();
            }

            _context.PlanesVuelo.Remove(planVuelo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanVueloExists(int id)
        {
            return _context.PlanesVuelo.Any(e => e.PlanVueloId == id);
        }


        // PUT: api/PlanVuelo/5/estado
        [HttpPut("{id}/controlManual")]
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] int nuevoModo)
        {
            var planVuelo = await _context.PlanesVuelo.FindAsync(id);

            if (planVuelo == null)
            {
                return NotFound();
            }

            planVuelo.ControlManual = nuevoModo;

            _context.Entry(planVuelo).Property(p => p.ControlManual).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();

                // Llamada api update planes de vuelo
                using (var httpClient = new HttpClient())
                {
                    var notificationApiUrl = $"http://localhost:5400/api/UpdatePlanesVuelo?id={id}";
                    var response = await httpClient.PostAsync(notificationApiUrl, null);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        return StatusCode((int)response.StatusCode, "Error al notificar la actualización del plan de vuelo.");
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanVueloExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}
