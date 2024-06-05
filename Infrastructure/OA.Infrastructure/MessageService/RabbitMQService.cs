using OA.Infrastructure.MessageSettings;
using RabbitMQ.Client;
using System.Text;

namespace OA.Infrastructure.MessageService
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService(IConnection connection, IModel channel)
        {
            _connection = connection;
            _channel = channel;
        }

        public RabbitMQService(IModel channel)
        {
            _channel = channel;
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: RabbitMqDefaults.ExchangeName, routingKey: RabbitMqDefaults.RoutingKey, basicProperties: null, body: body);
        }

        public void SendMessage(string exchangeName, string routingKey, byte[] body)
        {
            _channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }

}
