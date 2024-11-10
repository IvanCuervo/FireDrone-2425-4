using CentralBackend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

class DroneInfo
{
    public string? Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class UpdateMapController : ControllerBase
{
    private static DroneInfo[] _drones = new[]
    {
      new DroneInfo
      {
         Name = "Drone Gijón",
         Latitude = 43.53573,
         Longitude =  -5.66152
      },
      new DroneInfo
      {
         Name = "Drone Oviedo",
         Latitude = 43.3619145,
         Longitude = -5.8493887
      }
   };

    private readonly IHubContext<UpdateMapHub> _notificationHubContext;

    public UpdateMapController(IHubContext<UpdateMapHub> notificationHubContext)
    {
        _notificationHubContext = notificationHubContext;
    }

    [HttpPost]
    public async Task PostAsync()
    {
        foreach (var dron in _drones)
        {
            dron.Latitude += 0.001;
            dron.Longitude += 0.001;
        }
        string jsonString = JsonSerializer.Serialize(_drones);
        await _notificationHubContext.Clients.All.SendAsync("UpdateMapNotification", jsonString);
    }
}