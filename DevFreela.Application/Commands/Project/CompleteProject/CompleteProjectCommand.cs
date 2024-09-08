using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.Project.CompleteProject;

public record CompleteProjectCommand(int Id, string CreditCardNumber, string Cvv, string ExpiresAt, string FullName, decimal TotalCost) : IRequest<ResultViewModel>;