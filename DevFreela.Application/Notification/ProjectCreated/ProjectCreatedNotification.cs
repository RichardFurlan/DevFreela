using MediatR;

namespace DevFreela.Application.Notification.ProjectCreated;

public record ProjectCreatedNotification(int Id, string Title, decimal TotalCost) : INotification;
