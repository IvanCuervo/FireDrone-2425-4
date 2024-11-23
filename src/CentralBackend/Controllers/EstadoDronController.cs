using CentralBackend.Data;
using CentralBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult RecibirEstadoDron([FromBody] string message)
        {
            Console.WriteLine($"EstadoDron: {message}");

            return Ok();
        }

    }
}
