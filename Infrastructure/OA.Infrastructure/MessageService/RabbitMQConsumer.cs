using OA.Persistence.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OA.Infrastructure.Messaging
{
    public class RabbitMQConsumer<T> : IDisposable where T : class
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;
        private readonly IMongoDBRepository<T> _repository;

        public RabbitMQConsumer(IConnection connection, IModel channel, EventingBasicConsumer consumer, IMongoDBRepository<T> repository)
        {
            _connection = connection;
            _channel = channel;
            _consumer = consumer;
            _repository = repository;
        }

        public RabbitMQConsumer(string hostName, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
                // TODO: MongoDB'ye mesajı kaydetme işlemi yapılacak
                _repository.InsertAsync(message);
            };
            _channel.BasicConsume(queue: queueName,
                                  autoAck: true,
                                  consumer: _consumer);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
