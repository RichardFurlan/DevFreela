using DevFreela.Domain.Respositories;
using DevFreela.Domain.TransferObjects;
using MassTransit;

namespace DevFreela.Infrastructure.Services.MessageBus.Consumer;

public class PaymentApprovedConsumer : IConsumer<PaymentApprovedIntegrationEvent>
{
    private readonly IProjectRepository _projectRepository;
    public PaymentApprovedConsumer(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task Consume(ConsumeContext<PaymentApprovedIntegrationEvent> context)
    {
        var message = context.Message;

        var project = await _projectRepository.GetByIdAsync(message.IdProject);
        if (project != null)
        {
            project.Complete();
            await _projectRepository.UpdateAsync(project);
        }
    }
    
}