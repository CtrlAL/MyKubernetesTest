using NotificationService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IChannel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor) 
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
        }

        private async Task InitializeAsync()
        {
            if (_connection != null && _connection != null)
            {
                return;
            }

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
                _queueName = (await _channel.QueueDeclareAsync()).QueueName;
                await _channel.QueueBindAsync(queue: _queueName, exchange: "trigger", routingKey: "");

                Console.WriteLine("Listening on the Message Bus");

                _connection.ConnectionShutdownAsync += RabbitMq_ConnectionShutDown;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while create RabbitMQ connection: {ex.Message}");
                throw;
            }
        }

        private async Task RabbitMq_ConnectionShutDown(object sender, ShutdownEventArgs @event)
        {
            Console.WriteLine("Connection RabbitMq Shutdown");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            await InitializeAsync();

            if (_connection == null || _connection == null)
            {
                Console.WriteLine("Failed to connect");
                return;
            }

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += ReciveMessage;

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer, stoppingToken);
        }

        private async Task ReciveMessage(object sender, BasicDeliverEventArgs @event)
        {
            var body = @event.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
            await _eventProcessor.ProcessEvent(message);
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
