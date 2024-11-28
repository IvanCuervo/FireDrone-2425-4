using CentralBackend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using System;
using System.Text.Json;

public class DroneInfo
{
    public int DronId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public double Speed { get; set; }
    public double Battery { get; set; }
    public string Name { get; set; }

    //public int State { get; set; }
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
            return BadRequest("El objeto DroneInfo es inválido.");
        }

        // Buscar el dron en la lista usando el DronId
        var existingDrone = _drones.FirstOrDefault(d => d.DronId == updatedDrone.DronId);

        if (existingDrone == null)
        {
            // Si no existe, lo agrega a la lista
            _drones.Add(updatedDrone);
            Console.WriteLine($"Dron agregado con ID: {updatedDrone.DronId}");
        }
        else
        {
            // Si existe, actualiza sus propiedades
            existingDrone.Latitude = updatedDrone.Latitude;
            existingDrone.Longitude = updatedDrone.Longitude;
            existingDrone.Altitude = updatedDrone.Altitude;
            existingDrone.Speed = updatedDrone.Speed;
            existingDrone.Battery = updatedDrone.Battery;
            existingDrone.Name = updatedDrone.Name;

            Console.WriteLine($"Dron actualizado con ID: {updatedDrone.DronId}");
        }

        // Notificar a los clientes conectados sobre el cambio
        string jsonString = JsonSerializer.Serialize(_drones);
        await _notificationHubContext.Clients.All.SendAsync("UpdateMapNotification", jsonString);

        // Retorna la información actualizada o nueva del dron
        return Ok(updatedDrone);
    }

    [HttpGet("drones")]
    public IActionResult ObtenerDrones()
    {
        return Ok(_drones);
    }
}
