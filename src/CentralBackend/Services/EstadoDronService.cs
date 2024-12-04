using CentralBackend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.IO;
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
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:5400")
                };

                // Enviar la solicitud POST al endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync("api/updatemap/actualizar", dronEstado);

                // Verificar la respuesta del servidor
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Dron actualizado correctamente");
                }
                else
                {
                    Console.WriteLine($"Error al actualizar la posición del dron: {response.StatusCode}");
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Detalles del error: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ActualizarPosicionDron: {ex.Message}");
                throw;
            }
        }

    }
}