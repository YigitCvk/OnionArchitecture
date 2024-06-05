using OA.Infrastructure.MessageSettings;
using OA.Persistence.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OA.LogService.Worker
{
    public class Worker<T> : BackgroundService where T : class
    {
        private readonly ILogger<Worker<T>> _logger;
        private readonly IMongoDBRepository<T> _mongoDB;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public Worker(ILogger<Worker<T>> logger, IMongoDBRepository<T> mongoDB, IConfiguration configuration, IConnection connection, IModel channel) : this(logger, mongoDB, configuration)
        {
            _connection = connection;
            _channel = channel;
        }

        public Worker(ILogger<Worker<T>> logger, IMongoDBRepository<T> mongoDB, IConfiguration configuration)
        {
            _mongoDB = mongoDB;
            _logger = logger;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]),
                Ssl = new SslOption
                {
                    Enabled = bool.Parse(_configuration["RabbitMQ:Ssl:Enabled"]),
                    ServerName = _configuration["RabbitMQ:HostName"]
                }
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: RabbitMqDefaults.ExchangeName, type: ExchangeType.Fanout);
            channel.QueueDeclare(queue: RabbitMqDefaults.QueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            channel.QueueBind(queue: RabbitMqDefaults.QueueName,
                  exchange: RabbitMqDefaults.ExchangeName,
                  routingKey: string.Empty);
            Console.WriteLine(" [*] Waiting for messages.");
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[x] Received {message}");
                    await _mongoDB.InsertAsync(message);
                };
                channel.BasicConsume(queue: RabbitMqDefaults.QueueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
            await Task.FromCanceled(stoppingToken);
        }
    }
}
