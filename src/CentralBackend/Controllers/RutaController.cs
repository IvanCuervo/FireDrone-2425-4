using CentralBackend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CentralBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RutaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RutaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Ruta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ruta>>> GetRutas()
        {
            var rutas = await _context.Rutas.ToListAsync();
            return Ok(rutas);
        }
    }
}
