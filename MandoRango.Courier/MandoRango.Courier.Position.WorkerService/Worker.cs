using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MandoRango.Courier.Position.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var rabbitConfig = _configuration.GetSection("RabbitMQ");
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

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received {message} at {time}", message, DateTimeOffset.Now);
            };

            channel.BasicConsume(queue: courierPositionQueueName,
                     autoAck: true,
                     consumer: consumer);
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}