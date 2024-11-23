using Microsoft.AspNetCore.Mvc;

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


        public String CrearPlanVuelo([FromBody] int id)
        {
            EnviarPlanVuelo(_client);
            return "OK";
        }
        public void EnviarPlanVuelo(HttpClient client)
        {

            var response = client.PostAsync("api/ordenes/inicio", null).Result;
            Console.WriteLine("Resultado: "+response);
            
        }

    }
}