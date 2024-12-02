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

            Console.WriteLine(message);

            MedicionPlanVuelo medicion = new MedicionPlanVuelo
            {
                X = medicionDTO.Latitude,
                Y = medicionDTO.Longitude,
                Altura = medicionDTO.Altitude,
                Velocidad = medicionDTO.Speed,
                Fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //PlanVueloId = 1, // CAMBIAR
                PlanVuelo = planVuelo,
                ModoDeVuelo = "",
                SensoresActivados = "",
                Temperatura = 0.0,
                Humedad = 0,
                ImagenNormal = "",
                ImagenTermica = ""
            };


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5285");

            // Construir la URL completa
            string endpoint = "api/EstadoDron/recibirestado";
            string fullUrl = $"{client.BaseAddress}{endpoint}";

            // Imprimir la URL completa
            Console.WriteLine($"Enviando POST a la URL: {fullUrl}");

            HttpResponseMessage response = client.PostAsJsonAsync("api/EstadoDron/recibirestado", medicion).Result;
            if (response.IsSuccessStatusCode)
            {
                int idMedicion = response.Content.ReadFromJsonAsync<int>().Result;

                Console.WriteLine($"[x] Received MedicionPlanVueloId: {idMedicion} from topic: {topic}");

                // Incidencias
                Random random = new Random();
                int numRandom = random.Next(1, 11);
                string endpointIncidencia = "api/Incidencias";
                string fullUrlIncidencia = $"{client.BaseAddress}{endpointIncidencia}";
                Console.WriteLine($"Enviando POST a la URL: {fullUrlIncidencia}");
                Incidencia nuevaInc;
                if (numRandom < 5)
                {
                    // api incidencias
                    Console.WriteLine($"Enviar incidencia: {numRandom}");
                    if (numRandom < 3)
                    {
                        nuevaInc = new Incidencia
                        {
                            X = medicion.X,
                            Y = medicion.Y,
                            Informacion = "Alta temperatura",
                            Fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            MedicionPlanVueloId = idMedicion
                        };

                    }
                    else
                    {
                        nuevaInc = new Incidencia
                        {
                            X = medicion.X,
                            Y = medicion.Y,
                            Informacion = "Fuego detectado",
                            Fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            MedicionPlanVueloId = idMedicion
                        };
                    }
                    HttpResponseMessage responseIncidencia = client.PostAsJsonAsync("api/Incidencias", nuevaInc).Result;
                }

            }
            // Imprimir la respuesta
            Console.WriteLine($"Respuesta del servidor: {response.StatusCode}");
            Console.WriteLine($"Contenido de la respuesta: {response.Content.ReadAsStringAsync().Result}");

        }
    }
}
