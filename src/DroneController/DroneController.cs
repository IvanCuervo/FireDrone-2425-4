using Newtonsoft.Json;
using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DroneController
{
    public class DroneController
    {
        //Base
        string _droneID;
        string _droneDriver;
        IDroneDriver _drone;
        //Añadido
        const string EXCHANGE = "amq.topic";
        //Envio de status cada 2 segundos
        const int STATUS_INTERVAL = 2000;
        public int UpdateStatusInterval { get; set; }
        IConnection? _connection;
        IModel? _channel;
        string? _queueName;
        //Booleano para indicar si el dron envia informacion al sistema
        bool _sendStatus = false;

        public DroneController(string droneID, string droneDriver)
        {
            _droneID = droneID;
            _droneDriver = droneDriver;

            Log.Debug($"Drone controller {_droneID}-{_droneDriver} starting");

            // Crear cola de mensajes
            CreateMessageQueue(_droneID);

            // Instanciar driver de forma dinámica
            _drone = CreateDroneDriver(_droneDriver);
        }

        // Esperar a recibir mensajes del backend a través de la cola
        // Se procesaran en HandleDroneCommand
        public void Run()
        {
            //Recepcion de eventos
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                string commandtext = Encoding.UTF8.GetString(body);

                HandleDroneCommand(commandtext);
            };

            //Recepcion de datos 
            _channel.BasicConsume(queue: _queueName,
                autoAck: true,
                consumer: consumer);
        }

        public void Stop()
        {
            if (_connection != null && _channel != null)
            {
                _connection.CloseAsync();
                _connection.Dispose();
                _channel.Close();
                _channel.Dispose();
            }
        }

        // Se instancia el driver de forma dinámica. 
        // Debe haber una clase que implemente la interfaz IDroneDriver y cuyo nombre coincida con el driver
        // en el namespace del controlador
        private IDroneDriver CreateDroneDriver(string DroneDriver)
        {
            Type type = Type.GetType(GetType().Namespace + "." + DroneDriver);
            if (type == null)
            {
                throw new ArgumentException($"Error unable to find drone driver {DroneDriver}");
            }
            IDroneDriver drone = (IDroneDriver)Activator.CreateInstance(type);

            // Sería necesario publicar la información
            drone.SetUpdateCallback(new ConsoleDroneUpdate());

            return drone;
        }

        // Crear una cola para recibir comandos del backend control
        private void CreateMessageQueue(string DroneID)
        {
            var factory = new ConnectionFactory();
            String uri_rabbit = String.Format("amqp://guest:guest@{0}:5672", Environment.GetEnvironmentVariable("ASPNETCORE_URL_RABBIT"));
            factory.Uri = new Uri(uri_rabbit); //RABBITQ=amqp:////guest:guest@156.35.163.122:5672


            //Se crea la conexion
            _connection = factory.CreateConnectionAsync();
            _channel = _connection.CreateModel();
            _queueName = _channel.QueueDeclare();

            var _routingKey = "ControlBackend.Controller." + _droneID;
            _channel.QueueBind(queue: _queueName,
                exchange: EXCHANGE,
                routingKey: _routingKey);
        }

        // Enviar el estado a través de la cola para recibir al backend control
        private void SendStatus(string message)
        {
            if (_channel != null && _drone != null)
            {
                Dron statusActual = _drone.GetStatus();

                //Si el dron no esta volando no se envia informacion
                if (statusActual.status == DroneState.Landed)
                {
                    _sendStatus = false;
                }

                //imprimir en consola
                var body = Encoding.UTF8.GetBytes(message);
                Console.WriteLine("Estado del Dron: " + message);

                _channel.BasicPublish(exchange: EXCHANGE,
                    routingKey: "ControlBackend.Information." + _droneID,
                    mandatory: false,
                    basicProperties: null,
                    body: body);
            }
        }

        // Gestión de los mensajes de comandos recibidos por el controlador
        // Si se añaden más mensajes se debería gestionar con una tabla
        private void HandleDroneCommand(string commandtext)
        {
            // Decodificar el mensaje
            DroneCommand command = JsonConvert.DeserializeObject<DroneCommand>(commandtext);

            Log.Debug($"Executing drone command {command.Command}");

            if (command.Command == DroneCommand.START_FLIGHT_PLAN_CMD)
            {
                // Decodificar los argumentos
                Waypoint[] waypoints = JsonConvert.DeserializeObject<Waypoint[]>(command.Arguments);
                _drone.StartFlightPlan(waypoints);
            }
            else if (command.Command == DroneCommand.STOP_FLIGHT_PLAN_CMD)
            {
                _drone.StopFlightPlan();
            }
            else if (command.Command == DroneCommand.STATUS_CMD)
            {
                DroneStatus status = _drone.GetStatus();

                // Codificar el estado como JSON
                var statusStr = JsonConvert.SerializeObject(status);

                SendStatus(statusStr);
            }
        }
    }
}