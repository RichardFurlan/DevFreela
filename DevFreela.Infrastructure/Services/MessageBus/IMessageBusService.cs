namespace DevFreela.Infrastructure.Services.MessageBus;

public interface IMessageBusService
{
    Task PublishWithRetryAsync(string queue, byte[] message);
}