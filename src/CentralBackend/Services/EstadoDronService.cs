using Microsoft.AspNetCore.Mvc;

namespace CentralBackend.Services
{

    public class EstadoDronService
    {
        readonly HttpClient _client;


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

    }
}