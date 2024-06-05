using Microsoft.AspNetCore.Http;
using OA.Domain.Events;
using OA.Infrastructure.MessageService;
using System.Text.Json;

namespace OA.Application.Handlers.HealthCheck
{
    public class HealthCheck
    {
        private readonly RequestDelegate _next;
        private readonly IRabbitMQService _rabbitMQService;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public HealthCheck(RequestDelegate next, IRabbitMQService rabbitMQService)
        {
            _next = next;
            _rabbitMQService = rabbitMQService ?? throw new ArgumentNullException(nameof(rabbitMQService));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/health")
            {          

                var isHealthy = true;
                var message = new BaseEvent
                {
                   
                };               

                if (isHealthy)
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Healthy");
                    var messageJson = JsonSerializer.Serialize(message, _jsonOptions);
                    _rabbitMQService.SendMessage(messageJson);
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Unhealthy");                   
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
