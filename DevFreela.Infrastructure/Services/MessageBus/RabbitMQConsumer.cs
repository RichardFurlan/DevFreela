using System.Text;
using System.Text.Json;
using DevFreela.Domain.Respositories;
using DevFreela.Domain.TransferObjects;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DevFreela.Infrastructure.Services.MessageBus;

public class RabbitMQConsumer : IMessageConsumer
{
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private const string APPROVED_QUEUE = "Payments-Approved";
    public RabbitMQConsumer(string hostName, string userName, string password, IServiceProvider serviceProvider)
    {
        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _serviceProvider = serviceProvider;
    }

    public void Consume(string queue)
    {
        _channel.QueueDeclare(queue, false, false, false, null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            await ProcessMessageAsync(queue, message);

            _channel.BasicAck(args.DeliveryTag, false);
        };

        _channel.BasicConsume(queue, false, consumer);
    }
    
    private async Task ProcessMessageAsync(string queue, string message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            switch (queue)
            {
                case APPROVED_QUEUE:
                    var paymentApprovedEvent = JsonSerializer.Deserialize<PaymentApprovedIntegrationEvent>(message);
                    await CompleteProject(paymentApprovedEvent.IdProject, scope);
                    break;
            }
            
        }
    }

    private async Task CompleteProject(int idProject, IServiceScope scope)
    {
        var projectRepository = scope.ServiceProvider.GetRequiredService<IProjectRepository>();
        var project = await projectRepository.GetByIdAsync(idProject);
        if (project != null)
        {
            project.Complete();
            await projectRepository.UpdateAsync(project);
        }
        
    }
}