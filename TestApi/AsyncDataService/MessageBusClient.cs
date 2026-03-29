using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TaskService.Dto;

namespace TaskService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IChannel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishNewTask(TaskCreatedDto taskCreatedDto)
        {
            if (_connection == null || _channel == null)
            {
                await InitializeAsync();
            }

            if (_connection == null || _channel == null)
            {
                return;
            }

            var message = JsonSerializer.Serialize(taskCreatedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("Sending Message");
                await SendMessage(message);
            }
            else
            {
                Console.WriteLine("RabbitMq connections is closes while sending");
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        private async Task SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync<BasicProperties>(
                exchange: "trigger",
                routingKey: "",
                mandatory: false,
                basicProperties: new BasicProperties(),
                body: Encoding.UTF8.GetBytes(message).AsMemory()
            );

            Console.WriteLine("Message sended");
        }

        private async Task RabbitMq_ConnectionShutDown(object sender, ShutdownEventArgs @event)
        {
            Console.WriteLine("RabbitMq connection shut down");
        }

        private async Task InitializeAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMqHost"]!,
                Port = int.Parse(_configuration["RabbitMqPort"]!),
            };

            try
            {
                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                await _channel.ExchangeDeclareAsync(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdownAsync += RabbitMq_ConnectionShutDown;

                Console.WriteLine("Connected to RabbitMq");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while create RabbitMQ connection: {ex.Message}");
                throw;
            }
        }
    }
}
