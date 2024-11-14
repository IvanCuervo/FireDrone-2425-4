using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ControlBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesController : ControllerBase
    {


        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(queue: "ColaDron",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);


                    var queueName = channel.QueueDeclare().QueueName;

                    var corrId = Guid.NewGuid().ToString();
                    var props = channel.CreateBasicProperties();
                    props.ReplyTo = queueName;
                    props.CorrelationId = corrId;
                    props.Persistent = true;

                    Random a = new Random();
                    int numero = a.Next(5);
                    string message = numero.ToString();
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                    routingKey: "ColaDron",
                                    basicProperties: props,
                                    body: body);
                    Console.WriteLine("Enviado {0}", message);


                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        if (ea.BasicProperties.CorrelationId == corrId)
                        {
                            string messagerecv = Encoding.UTF8.GetString(ea.Body.ToArray());
                            Console.WriteLine("Recibido {0}", messagerecv);
                        }

                    };

                    channel.BasicConsume(queue: queueName,
                                      autoAck: false,
                                      consumer: consumer);
                    Console.WriteLine("Press enter to exit");
                    Console.ReadLine();


                }
            }

            return new string[] { "value1", "value2" };
        }
    }
}
