using DevFreela.Domain.DTOs;

namespace DevFreela.Infrastructure.Services.PaymentService;

public interface IPaymentService
{
    void ProcessPayment(PaymentInfoDTO paymentInfoDto);
}