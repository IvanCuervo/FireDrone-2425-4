using CentralBackend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[Route("api/[controller]")]
[ApiController]
public class UpdatePlanesVueloController : ControllerBase
{

    private readonly IHubContext<UpdatePlanesVueloHub> _notificationHubContext;

    public UpdatePlanesVueloController(IHubContext<UpdatePlanesVueloHub> notificationHubContext)
    {
        _notificationHubContext = notificationHubContext;
    }

    [HttpPost]
    public async Task PostAsync(int id)
    {
        await _notificationHubContext.Clients.All.SendAsync("UpdatePlanesVueloNotification", id);
    }
}