using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using RabbitMQ.Client;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var configurations = app.Services.GetRequiredService<IConfiguration>();

app.MapPost("/CourierPosition", (CourierPosition position) =>
{
    if(
        position.CourierId == Guid.Empty
        || (position.Latitude == 0 && position.Longitude == 0)
        || position.Latitude == double.MinValue
        || position.Longitude == double.MinValue
    )
    {
        return Results.BadRequest("Invalid position");
    }
    
    var rabbitConfig = configurations.GetSection("RabbitMQ");
    var rabbitHostName = rabbitConfig.GetValue<string>("HostName");
    var courierPositionQueueName = rabbitConfig.GetValue<string>("CourierPositionQueue");


    var factory = new ConnectionFactory { HostName = rabbitHostName };
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();


    channel.QueueDeclare(queue: courierPositionQueueName,
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    string message = JsonSerializer.Serialize(position);
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: courierPositionQueueName,
                         basicProperties: null,
                         body: body);

    return Results.NoContent();
})
.WithName("SendCourierPosition");

app.Run();



internal record struct CourierPosition(Guid CourierId, double Latitude, double Longitude)
{
}