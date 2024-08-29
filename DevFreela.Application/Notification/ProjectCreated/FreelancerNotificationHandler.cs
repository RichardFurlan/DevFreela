using MediatR;

namespace DevFreela.Application.Notification.ProjectCreated;

public class FreelancerNotificationHandler : INotificationHandler<ProjectCreatedNotification>
{
    public Task Handle(ProjectCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Notificando os freelancer sobre o projeto {notification.Title}");
        
        return Task.CompletedTask;
    }
}