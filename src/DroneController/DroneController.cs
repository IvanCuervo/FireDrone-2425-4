using Azure;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.Impl;
using System.Text;

namespace DroneController
{
    public class DroneController
    {
        
        string _droneID;
        string _droneDriver;

        IDroneDriver _drone;

        const string EXCHANGE = "amq.topic";
        IConnection _connection;
        IModel _channel;
        string _queueName;
        bool _sendStatusPeriodically = false;
        const int SEND_STATUS_INTERVAL = 3000;



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

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
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
            if (_connection != null && _channel != null){
                _connection.Close();
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

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = _channel.QueueDeclare();

            var _routingKey = "ColaDron" + _droneID;

            _channel.QueueBind(queue: _queueName,
                exchange: EXCHANGE,
                routingKey: _routingKey);
        }

        // Enviar el estado a través de la cola para recibir al backend control
        private void SendStatus(string message)
        {
            if (_channel != null && _drone != null) { 
                    
                DroneStatus estado = _drone.GetStatus();

                var body = Encoding.UTF8.GetBytes(message);
                Console.WriteLine("Estado del Dron: " + message);

                _channel.BasicPublish(exchange: EXCHANGE,
                    routingKey: "Estado." + _droneID,
                    mandatory: false,
                    basicProperties: null,
                    body: body);

            }
        }

        //Envia el estado continuamente
        private void SendStatusPerodically()
        {

            if (_drone != null)
            {
                while (_sendStatusPeriodically)
                {                    
                    DroneStatus estado = _drone.GetStatus();
                    var estadoJSON = JsonConvert.SerializeObject(estado);

                    //Envio del estado a traves de la API
                    SendStatus(estadoJSON);
                    //intervalo de tiempo entre envios de estados
                    System.Threading.Thread.Sleep(SEND_STATUS_INTERVAL);
                }
            }
        }

        // Gestión de los mensajes de comandos recibidos por el controlador
        // Si se añaden más mensajes se debería gestionar con una tabla
        private void HandleDroneCommand(string commandtext)
        {

           // Decodificar el mensaje
           DroneCommand command = JsonConvert.DeserializeObject<DroneCommand>(commandtext);

            Log.Debug($"Executing drone command {command.Command}");
            Thread hiloEnvioEstado;

            if (command.Command == DroneCommand.START_FLIGHT_PLAN_CMD)
            {
                // Decodificar los argumentos
                Waypoint[] waypoints = JsonConvert.DeserializeObject<Waypoint[]>(command.Arguments);
                _drone.StartFlightPlan(waypoints);

                _sendStatusPeriodically = true;

                hiloEnvioEstado = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;

                    SendStatusPerodically();
                });

                hiloEnvioEstado.Start();

            }
            else if (command.Command == DroneCommand.STOP_FLIGHT_PLAN_CMD)
            {
                _drone.StopFlightPlan();
                _sendStatusPeriodically = false;
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