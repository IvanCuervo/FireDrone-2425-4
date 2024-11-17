using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CentralBackend.Data;
using Models;

namespace CentralBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicionPlanVueloController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicionPlanVueloController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicionPlanVuelo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicionPlanVuelo>>> GetMedicionesPlanVuelo()
        {
            return await _context.MedicionesPlanVuelo.ToListAsync();
        }

        // GET: api/MedicionPlanVuelo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicionPlanVuelo>> GetMedicionPlanVuelo(int id)
        {
            var medicionPlanVuelo = await _context.MedicionesPlanVuelo.FindAsync(id);

            if (medicionPlanVuelo == null)
            {
                return NotFound();
            }

            return medicionPlanVuelo;
        }

        // PUT: api/MedicionPlanVuelo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicionPlanVuelo(int id, MedicionPlanVuelo medicionPlanVuelo)
        {
            if (id != medicionPlanVuelo.MedicionPlanVueloId)
            {
                return BadRequest();
            }

            _context.Entry(medicionPlanVuelo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicionPlanVueloExists(id))
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

        // POST: api/MedicionPlanVuelo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MedicionPlanVuelo>> PostMedicionPlanVuelo(MedicionPlanVuelo medicionPlanVuelo)
        {
            _context.MedicionesPlanVuelo.Add(medicionPlanVuelo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicionPlanVuelo", new { id = medicionPlanVuelo.MedicionPlanVueloId }, medicionPlanVuelo);
        }

        // DELETE: api/MedicionPlanVuelo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicionPlanVuelo(int id)
        {
            var medicionPlanVuelo = await _context.MedicionesPlanVuelo.FindAsync(id);
            if (medicionPlanVuelo == null)
            {
                return NotFound();
            }

            _context.MedicionesPlanVuelo.Remove(medicionPlanVuelo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicionPlanVueloExists(int id)
        {
            return _context.MedicionesPlanVuelo.Any(e => e.MedicionPlanVueloId == id);
        }
    }
}
