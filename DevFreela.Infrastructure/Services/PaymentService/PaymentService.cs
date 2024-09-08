using System.Text;
using System.Text.Json;
using DevFreela.Domain.TransferObjects;
using DevFreela.Infrastructure.Services.MessageBus;
using Microsoft.Extensions.Hosting;


namespace DevFreela.Infrastructure.Services.PaymentService;

public class PaymentService : BackgroundService, IPaymentService
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly IMessageConsumer _messageConsumer;
    private const string QUEUE_NAME = "Payments";
    private const string APPROVED_QUEUE = "Payments-Approved";
    public PaymentService(IMessagePublisher  messagePublisher, IMessageConsumer messageConsumer)
    {
        _messagePublisher = messagePublisher;
        _messageConsumer = messageConsumer;
    }
    public async Task ProcessPaymentAsync(PaymentInfoDTO paymentInfoDto)
    {
        var paymentInfoJson = JsonSerializer.Serialize(paymentInfoDto);

        var paymentInfoBytes = Encoding.UTF8.GetBytes(paymentInfoJson);
        await _messagePublisher.PublishWithRetryAsync(QUEUE_NAME, paymentInfoBytes);
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageConsumer.Consume(APPROVED_QUEUE);
        return Task.CompletedTask;
    }
}