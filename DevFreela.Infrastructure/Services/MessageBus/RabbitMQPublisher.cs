using RabbitMQ.Client;

namespace DevFreela.Infrastructure.Services.MessageBus;

public class RabbitMQPublisher : IMessagePublisher
{
    private readonly IModel _channel;
    private const int MAX_RETRIES = 3;
    private const int DELAY_IN_SECONDS = 2;

    public RabbitMQPublisher(string hostName, string userName, string password)
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

    public async Task PublishWithRetryAsync(string queue, byte[] message)
    {
        await RetryAsync(async () =>
        {
            Publish(queue, message);
        }, MAX_RETRIES, DELAY_IN_SECONDS);
    }
    
    private void Publish(string queue, byte[] message)
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

    private async Task RetryAsync(Func<Task> action, int maxRetries = 3, int delayInSeconds = 2)
    {
        int attempt = 0;
        while (attempt < maxRetries)
        {
            try
            {
                await action();
                return;
            }
            catch
            {
                attempt++;
                if (attempt >= maxRetries)
                {
                    throw;
                }

                await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));
            }
        }
    }
}