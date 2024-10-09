using System.Text;
using System.Text.Json;
using DevFreela.Domain.TransferObjects;
using MassTransit;

namespace DevFreela.Infrastructure.Services.MessageBus.Publisher;

public class RabbitMQPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RabbitMQPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishWithRetryAsync(string queue, byte[] message)
    {
        var messageString = Encoding.UTF8.GetString(message);
        var paymentInfo = JsonSerializer.Deserialize<PaymentInfoDTO>(messageString);
        
        if (paymentInfo != null)
        {
            await _publishEndpoint.Publish(paymentInfo);
        }
    }
    

}