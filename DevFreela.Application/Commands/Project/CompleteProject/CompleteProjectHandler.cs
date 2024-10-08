using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using DevFreela.Domain.TransferObjects;
using DevFreela.Infrastructure.Services.PaymentService;
using MediatR;

namespace DevFreela.Application.Commands.Project.CompleteProject;

public class CompleteProjectHandler : IRequestHandler<CompleteProjectCommand, ResultViewModel>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IPaymentService _paymentService;
    public CompleteProjectHandler(IProjectRepository projectRepository, IPaymentService paymentService)
    {
        _projectRepository = projectRepository;
        _paymentService = paymentService;
    }
    public async Task<ResultViewModel> Handle(CompleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }

        var paymentInfoDto = new PaymentInfoDTO(project.Id, request.CreditCardNumber, request.Cvv, request.ExpiresAt,
            request.FullName, project.TotalCost);
        
        await _paymentService.ProcessPaymentAsync(paymentInfoDto);
        
        var isSuccess = project.SetPaymentPending();
        if (!isSuccess)
        {
            return ResultViewModel.Error("O projeto não está em andamento, portanto não pode ser alterado para pagamento pendente.");
        }

        await _projectRepository.UpdateAsync(project);

        return ResultViewModel.Success();
    }
}