using ControlBackend.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ControlBackend.Broker
{
    public class RabbitMqBroker : IBroker, IDisposable
    {
        const string EXCHANGE = "amq.topic";
        ConnectionFactory _factory;
        IConnection _connection;
        IModel _channel;

        public RabbitMqBroker(string host, string user, string pass)
        {
            _factory = new ConnectionFactory()
            {
                HostName = host,
                UserName = user,
                Password = pass
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }

        public string Subscribe(string topic, ISubscriber sub)
        {
            var queueName = _channel.QueueDeclare();

            _channel.QueueBind(queue: queueName,
               exchange: EXCHANGE,
               routingKey: topic);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var routingKey = ea.RoutingKey;
                sub.OnMessage(routingKey, body);
            };
            var consumerTag = _channel.BasicConsume(queue: queueName,
                            autoAck: true,
                            consumer: consumer);
            return consumerTag;
        }

        public void CancelRecv(string consumerTag)
        {
            _channel.BasicCancel(consumerTag);
        }

        public void Publish(string topic, byte[] message)
        {
            _channel.BasicPublish(exchange: EXCHANGE,
                        routingKey: topic,
                        basicProperties: null,
                        body: message);
        }
    }
}
