using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost"
};

var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare("orders", durable: true, false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (_, eventArg) =>
{
    var body = eventArg.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);
};

channel.BasicConsume(queue: "orders", true, consumer);
Console.ReadLine();