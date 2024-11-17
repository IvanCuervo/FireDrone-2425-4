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
    public class EstacionControlController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstacionControlController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EstacionControl
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstacionControl>>> GetEstacionesControl()
        {
            return await _context.EstacionesControl.ToListAsync();
        }

        // GET: api/EstacionControl/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstacionControl>> GetEstacionControl(int id)
        {
            var estacionControl = await _context.EstacionesControl.FindAsync(id);

            if (estacionControl == null)
            {
                return NotFound();
            }

            return estacionControl;
        }

        // PUT: api/EstacionControl/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstacionControl(int id, EstacionControl estacionControl)
        {
            if (id != estacionControl.EstacionControlId)
            {
                return BadRequest();
            }

            _context.Entry(estacionControl).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstacionControlExists(id))
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

        // POST: api/EstacionControl
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstacionControl>> PostEstacionControl(EstacionControl estacionControl)
        {
            _context.EstacionesControl.Add(estacionControl);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstacionControl", new { id = estacionControl.EstacionControlId }, estacionControl);
        }

        // DELETE: api/EstacionControl/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstacionControl(int id)
        {
            var estacionControl = await _context.EstacionesControl.FindAsync(id);
            if (estacionControl == null)
            {
                return NotFound();
            }

            _context.EstacionesControl.Remove(estacionControl);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstacionControlExists(int id)
        {
            return _context.EstacionesControl.Any(e => e.EstacionControlId == id);
        }
    }
}
