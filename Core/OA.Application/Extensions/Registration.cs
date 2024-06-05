using BvysAPI.Application.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OA.Application.Handlers.HealthCheck;
using OA.Application.Mappers;
using OA.Domain.Settings;
using OA.Infrastructure.MessageService;
using OA.Infrastructure.Messaging;
using OA.Persistence.Repositories;
using RabbitMQ.Client;
using System.Reflection;

namespace OA.Application.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var assm = Assembly.GetExecutingAssembly();

            services.AddMediatR(assm);
            services.AddAutoMapper(assm);
            services.AddAutoMapper(typeof(CustomMapping).Assembly);
            services.AddScoped(typeof(IMongoDBRepository<>), typeof(MongoDBRepository<>));
            services.AddSingleton<IRabbitMQService>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var settings = config.GetSection("RabbitMQ").Get<RabbitMQSettings>();

                // Establish connection and channel
                var factory = new ConnectionFactory() { HostName = settings.HostName, Port = settings.Port, Password = settings.Password };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                return new RabbitMQService(channel); // Pass only the channel
            });

            var config = configuration.GetSection(nameof(OaSettings));

            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(assm));

            return services;
        }

        public static void AddInfrastructureServices(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
            collection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            collection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            collection.AddScoped<OaSettings>();
            collection.AddScoped(typeof(IMongoDBRepository<>), typeof(MongoDBRepository<>));
        }

        public static IApplicationBuilder UseHealthCheckMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HealthCheck>();
        }
    }
}
