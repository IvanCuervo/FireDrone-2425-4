using ControlBackend.Interfaces;
using Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace ControlBackend.Servicios
{
    public class ReceptorMensajes : ISubscriber
    {
        public void OnMessage(string topic, byte[] body)
        {
            // Decodificar el mensaje recibido
            var message = Encoding.UTF8.GetString(body);

            // Deserializar el mensaje como MedicionPlanVuelo
            MedicionPlanVueloDTO medicionDTO = JsonConvert.DeserializeObject<MedicionPlanVueloDTO>(message);

            if (medicionDTO == null)
            {
                Console.WriteLine("Error: La medición deserializada es nula.");
                return;
            }

            var dronIDString = topic.Split('.')[1];

            int dronID = Int32.Parse(dronIDString);


            var planVuelo = new PlanVuelo
            {
                DronId = dronID, // Asignar el DronId desde el DTO
            };


            MedicionPlanVuelo medicion = new MedicionPlanVuelo
            {
                X = medicionDTO.Latitude,
                Y = medicionDTO.Longitude,
                Altura = medicionDTO.Altitude,
                Velocidad = medicionDTO.Speed,
                Fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                PlanVuelo = planVuelo
            };


            Console.WriteLine($"[x] Received MedicionPlanVueloId: {medicion.MedicionPlanVueloId} from topic: {topic}");


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5285");

            // Construir la URL completa
            string endpoint = "api/EstadoDron/recibirestado";
            string fullUrl = $"{client.BaseAddress}{endpoint}";

            // Imprimir la URL completa
            Console.WriteLine($"Enviando POST a la URL: {fullUrl}");

            HttpResponseMessage response = client.PostAsJsonAsync("api/EstadoDron/recibirestado", medicion).Result;

            // Imprimir la respuesta
            Console.WriteLine($"Respuesta del servidor: {response.StatusCode}");
            Console.WriteLine($"Contenido de la respuesta: {response.Content.ReadAsStringAsync().Result}");

        }
    }
}
