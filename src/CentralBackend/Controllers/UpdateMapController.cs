using CentralBackend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

public class DroneInfo
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public double Speed { get; set; }
    public double Battery { get; set; }
    public int State { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class UpdateMapController : ControllerBase
{
    private static List<DroneInfo> _drones = new List<DroneInfo>(); // Lista vacía para gestionar drones dinámicamente

    private readonly IHubContext<UpdateMapHub> _notificationHubContext;

    public UpdateMapController(IHubContext<UpdateMapHub> notificationHubContext)
    {
        _notificationHubContext = notificationHubContext;
    }

    [HttpPost("actualizar")]
    public async Task<IActionResult> ActualizarDron([FromBody] DroneInfo updatedDrone)
    {
        // Validar el objeto recibido
        if (updatedDrone == null)
        {
            return BadRequest("El objeto DroneInfo es inválido");
        }

        // Busca el dron en la lista
        var existingDrone = _drones.FirstOrDefault(d => d.Altitude == 100.0);

        if (existingDrone == null)
        {
            // Si no existe, lo agrega
            _drones.Add(updatedDrone);
        }
        else
        {
            // Si existe, actualiza su posición
            existingDrone.Latitude = updatedDrone.Latitude;
            existingDrone.Longitude = updatedDrone.Longitude;
        }

        // Notificar a los clientes conectados
        string jsonString = JsonSerializer.Serialize(new[] { updatedDrone });
        await _notificationHubContext.Clients.All.SendAsync("UpdateMapNotification", jsonString);

        return Ok(updatedDrone); // Retorna la información actualizada del dron
    }


    [HttpGet("drones")]
    public IActionResult ObtenerDrones()
    {
        return Ok(_drones);
    }
}
