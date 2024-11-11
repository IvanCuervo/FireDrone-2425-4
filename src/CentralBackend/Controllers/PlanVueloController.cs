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

namespace CentralBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanVueloController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlanVueloController(AppDbContext context)
        {
            _context = context;
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
                return CreatedAtAction(nameof(GetPlanVuelo), new { id = planVuelo.PlanVueloId }, planVuelo);
            }
            else
            {
                return StatusCode(500, "Error al guardar el plan de vuelo.");
            }

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
    }
}
