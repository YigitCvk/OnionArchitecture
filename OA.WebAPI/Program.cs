using MediatR;
using Microsoft.OpenApi.Models;
using OA.Application.Extensions;
using OA.Domain.Settings;
using OA.Persistence.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OA API ", Version = "v2", Description = "OA Api Request", Contact = new OpenApiContact { Name = "Yiğit Çevik", Email ="me@yigitcevik.dev" } });
});
//MongoDb
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
//Mediatr
builder.Services.AddMediatR(typeof(Registration).Assembly);

var configuration = builder.Configuration;

builder.Services.AddApplicationRegistration(configuration);
builder.Services.AddInfrastructureServices(configuration);
Utils.SetBvysSettings(builder.Configuration.GetSection(nameof(OaSettings)).Get<OaSettings>());
builder.Services.AddScoped<OaSettings>();
//builder.Services.AddHostedService<LogConsumer>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OA API V2");
        c.RoutePrefix = string.Empty;
    });
}
app.UseRouting();

app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();