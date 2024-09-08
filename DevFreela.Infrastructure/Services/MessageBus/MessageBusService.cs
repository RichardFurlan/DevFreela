using System.Security.Cryptography;
using RabbitMQ.Client;

namespace DevFreela.Infrastructure.Services.MessageBus;

public class MessageBusService : IMessageBusService
{
    private readonly IModel _channel;
    public MessageBusService(string hostName, string userName, string password)
    {
        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
    }
    public void Publish(string queue, byte[] message)
    {
        _channel.QueueDeclare(
            queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        _channel.BasicPublish(
            exchange: "",
            routingKey: queue,
            basicProperties: null,
            body: message
            );
        
    }
}