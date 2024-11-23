using CentralBackend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CentralBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstacionBaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstacionBaseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EstacionBase
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstacionBase>>> GetEstacionesBase()
        {
            var estacionesBase = await _context.EstacionesBase.ToListAsync();
            return Ok(estacionesBase);
        }

        // GET: api/EstacionesBase/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstacionBase>> GetEstacionBase(int id)
        {
            var estacionBase = await _context.EstacionesBase.FindAsync(id);

            if (estacionBase == null)
            {
                return NotFound();
            }

            return estacionBase;
        }
    }
}
