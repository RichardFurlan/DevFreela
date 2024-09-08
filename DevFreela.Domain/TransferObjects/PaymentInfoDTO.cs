namespace DevFreela.Domain.TransferObjects;

public record PaymentInfoDTO(int IdProject, string CreditCardNumber, string Cvv, string ExpiresAt, string FullName, decimal TotalCost);