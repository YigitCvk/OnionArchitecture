using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OA.Domain.Entities;
using OA.Infrastructure.MessageService;
using OA.LogService.Worker;
using OA.Persistence.Repositories;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddHostedService<Worker<RabbitLog>>();

        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"],
                Port = int.Parse(configuration["RabbitMQ:Port"]),
                Ssl = new SslOption
                {
                    Enabled = bool.Parse(configuration["RabbitMQ:Ssl:Enabled"]),
                    ServerName = configuration["RabbitMQ:HostName"]
                }
            };
            return factory.CreateConnection();
        });

        services.AddSingleton<IModel>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            return connection.CreateModel();
        });

        services.AddSingleton<IRabbitMQService, RabbitMQService>();

        services.AddSingleton(typeof(IMongoDBRepository<>), typeof(MongoDBRepository<>));
    })
    .Build();

host.Run();
