using MottuChallenge.API.Entities;
using MottuChallenge.API.Infrastructure.Persistence;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;

namespace MottuChallenge.API.Workers
{
    public class MotorcycleCreatedConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private const string QueueName = "motorcycle-created";

        public MotorcycleCreatedConsumer(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var connectionString = configuration.GetConnectionString("RabbitMQ");
            var factory = new ConnectionFactory() { Uri = new Uri(connectionString) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var motorcycle = JsonSerializer.Deserialize<MotorcycleEntity>(message);

                if (motorcycle?.Year == 2024)
                {
                    // Cria um escopo de serviço para obter o DbContext
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var notification = new NotificationEntity { MotorcycleId = motorcycle.Id };
                        await dbContext.NotificationEntities.AddAsync(notification);
                        await dbContext.SaveChangesAsync();
                    }
                }
                
                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };
    
            _channel.BasicConsume(QueueName, false, consumer);
            return Task.CompletedTask;
        }
    }
}
