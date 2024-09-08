using DevFreela.Domain.TransferObjects;

namespace DevFreela.Infrastructure.Services.PaymentService;

public interface IPaymentService
{
    Task ProcessPaymentAsync(PaymentInfoDTO paymentInfoDto);
}