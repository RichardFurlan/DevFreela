using System.Text;
using System.Text.Json;
using DevFreela.Domain.TransferObjects;
using DevFreela.Infrastructure.Services.MessageBus;

namespace DevFreela.Infrastructure.Services.PaymentService;

public class PaymentService : IPaymentService
{
    private readonly IMessageBusService _messageBusService;
    private const string QUEUE_NAME = "Payments";
    public PaymentService(IMessageBusService messageBusService)
    {
        _messageBusService = messageBusService;
    }
    public async Task ProcessPaymentAsync(PaymentInfoDTO paymentInfoDto)
    {
        var paymentInfoJson = JsonSerializer.Serialize(paymentInfoDto);

        var paymentInfoBytes = Encoding.UTF8.GetBytes(paymentInfoJson);
        await _messageBusService.PublishWithRetryAsync(QUEUE_NAME, paymentInfoBytes);
    }
}