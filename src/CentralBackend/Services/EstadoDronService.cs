using CentralBackend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace CentralBackend.Services
{

    public class EstadoDronService
    {
        readonly HttpClient _client;
        private readonly IHubContext<UpdateMapHub> _hubContext;


        // Define the constructor that takes both a database context and an HTTP client as arguments
        public EstadoDronService(HttpClient client)
        {
            _client = client;
        }


        public String EstadoDron([FromBody] int id)
        {
            EnviarEstadoDron(_client, id);
            return "OK";
        }
        public void EnviarEstadoDron(HttpClient client, int id)
        {

            var response = client.PostAsync("api/ordenes/estadodron/"+ id, null).Result;
            
        }

        public async Task ActualizarPosicionDron(DroneInfo dronEstado)
        {
            try
            {
                // Serializar la información del dron y enviarla al controlador UpdateMapController
                var content = new StringContent(JsonSerializer.Serialize(dronEstado), System.Text.Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("api/updatemap/actualizar", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al actualizar la posición del dron: {response.StatusCode}");
                }

                Console.WriteLine($"Dron actualizado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ActualizarPosicionDron: {ex.Message}");
                throw;
            }
        }
    }
}