using Contracts.Common.Interfaces;
using Contracts.Messages;
using RabbitMQ.Client;

using System.Text;

namespace Infrastructure.Messages
{
    public class RabbitMQPublisher : IMessagePublisher
    {
        private readonly ISerializeService _serializeService;

        public RabbitMQPublisher(ISerializeService serializeService)
        {
            _serializeService = serializeService;
        }

        public void SendMessage<T>(T message)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
            };

            var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("orders", durable: true, exclusive: false);

            var jsonData = _serializeService.Serialize<T>(message);
            var body = Encoding.UTF8.GetBytes(jsonData);

            channel.BasicPublish("", routingKey: "orders", body: body);
        }
    }
}

