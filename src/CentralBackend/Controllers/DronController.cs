﻿using CentralBackend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CentralBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DronController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DronController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Dron
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dron>>> GetDrones()
        {
            var drones = await _context.Drones.ToListAsync();
            return Ok(drones);
        }
    }
}