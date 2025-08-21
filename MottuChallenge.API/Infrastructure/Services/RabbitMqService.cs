using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace MottuChallenge.API.Services
{
    public class RabbitMqService : IMessagingService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RabbitMQ");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A string de conexão 'RabbitMQ' não foi encontrada.");
            }
            
            var factory = new ConnectionFactory() { Uri = new Uri(connectionString), DispatchConsumersAsync = true };
            
            var retries = 5;
            while (retries > 0)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    Console.WriteLine("--> Conectado ao RabbitMQ com sucesso.");
                    break;
                }
                catch (BrokerUnreachableException)
                {
                    retries--;
                    Console.WriteLine($"--> Não foi possível conectar ao RabbitMQ. Tentando novamente em 5 segundos... ({retries} tentativas restantes)");
                    Thread.Sleep(5000);
                }
            }

            if (_connection == null)
            {
                throw new InvalidOperationException("Não foi possível conectar ao RabbitMQ após várias tentativas.");
            }
        }

        public void Publish(string queue, object message)
        {
            _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            
            var jsonMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            _channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
        }
    }
}