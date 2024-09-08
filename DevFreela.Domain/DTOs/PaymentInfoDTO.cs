namespace DevFreela.Domain.DTOs;

public record PaymentInfoDTO(int IdProject, string CreditCardNumber, string Cvv, string ExpiresAt, string FullName, decimal TotalCost);