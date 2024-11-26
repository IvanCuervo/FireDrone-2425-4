using ControlBackend.Interfaces;
using DroneController;
using Models;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System;
using System.Text;

namespace ControlBackend.Servicios
{
    public class ReceptorMensajes : ISubscriber
    {


        public void OnMessage(string topic, byte[] body)
        {
            var message = Encoding.UTF8.GetString(body);

            Dron dronStatus = JsonConvert.DeserializeObject<Dron>(message);

            var dronID = topic.Split('.')[1];


            Console.WriteLine($"[x] Received DronID:'{dronID}'Message :'{message}'");

            dronStatus.DronId = Int32.Parse(dronID);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5285"); // Asegura que el host y puerto sean correctos.


            HttpResponseMessage response = client.PostAsJsonAsync("api/EstadoDron/recibirestado", dronStatus).Result;


        }
    }
}
