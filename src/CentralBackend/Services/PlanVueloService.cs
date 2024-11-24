using Microsoft.AspNetCore.Mvc;
using Models;

namespace CentralBackend.Services
{

    public class PlanVueloService
    {
        readonly HttpClient _client;


        // Define the constructor that takes both a database context and an HTTP client as arguments
        public PlanVueloService(HttpClient client)
        {
            _client = client;
        }


        public String CrearPlanVuelo([FromBody] Conexion conexion)
        {
            EnviarPlanVuelo(_client, conexion);
            return "OK";
        }

        public String PararVuelo([FromBody] Conexion conexion)
        {
            PararPlanVuelo(_client, conexion);
            return "OK";
        }

        public void EnviarPlanVuelo(HttpClient client, Conexion conexion)
        {
            var response = client.PostAsJsonAsync("api/ordenes/inicio", conexion).Result;
        }

        public void PararPlanVuelo(HttpClient client, Conexion conexion)
        {
            var response = client.PostAsJsonAsync("api/ordenes/parar", conexion).Result;
        }

    }
}