using System.Text;
using System.Text.Json;
using DevFreela.Domain.TransferObjects;
using DevFreela.Infrastructure.Services.MessageBus;
using DevFreela.Infrastructure.Services.MessageBus.Publisher;
using Microsoft.Extensions.Hosting;


namespace DevFreela.Infrastructure.Services.PaymentService;

public class PaymentService : BackgroundService, IPaymentService
{
    private readonly IMessagePublisher _messagePublisher;
    private const string QUEUE_NAME = "Payments";

    public PaymentService(IMessagePublisher  messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }
    public async Task ProcessPaymentAsync(PaymentInfoDTO paymentInfoDto)
    {
        var paymentInfoJson = JsonSerializer.Serialize(paymentInfoDto);

        var paymentInfoBytes = Encoding.UTF8.GetBytes(paymentInfoJson);
        await _messagePublisher.PublishWithRetryAsync(QUEUE_NAME, paymentInfoBytes);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}