using System.Text;
using DevFreela.Domain.IntegrationEvents;
using DevFreela.Domain.Respositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DevFreela.Application.Consumers;

public class PaymentApprovedConsumer : BackgroundService
{
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private const string PAYMENT_APPROVED_QUEUE = "Payments-Approved";
    public PaymentApprovedConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        
        _channel.QueueDeclare(
            queue: PAYMENT_APPROVED_QUEUE,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, eventArgs) =>
        {
            var paymentApprovedBytes = eventArgs.Body.ToArray();
            var paymentApprovedJson = Encoding.UTF8.GetString(paymentApprovedBytes);

            var paymentApprovedIntegrationEvent = JsonSerializer.Deserialize<PaymentApprovedIntegrationEvent>(paymentApprovedJson);

            await CompleteProject(paymentApprovedIntegrationEvent.IdProject);
            
            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        _channel.BasicConsume(PAYMENT_APPROVED_QUEUE, false, consumer);
        
        return Task.CompletedTask;
    }

    private async Task CompleteProject(int IdProject)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var projectRepository = scope.ServiceProvider.GetRequiredService<IProjectRepository>();
            var project = await projectRepository.GetByIdAsync(IdProject);

            project.Complete();

            projectRepository.UpdateAsync(project);
        }
    }
}