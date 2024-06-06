using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OA.Domain.Entities;
using OA.Infrastructure.MessageService;
using OA.LogService.Worker;
using OA.Persistence.Repositories;
using RabbitMQ.Client;
using System;

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
            var hostName = configuration["RabbitMQ:HostName"] ?? "default-hostname";
            var userName = configuration["RabbitMQ:UserName"] ?? "default-username";
            var password = configuration["RabbitMQ:Password"] ?? "default-password";
            var portStr = configuration["RabbitMQ:Port"] ?? "5672";
            var sslEnabledStr = configuration["RabbitMQ:Ssl:Enabled"] ?? "false";

            int port = int.Parse(portStr);
            bool sslEnabled = bool.Parse(sslEnabledStr);

            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = port,
                Ssl = new SslOption
                {
                    Enabled = sslEnabled,
                    ServerName = hostName
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
