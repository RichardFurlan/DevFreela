namespace DevFreela.Infrastructure.Services.MessageBus;

public interface IMessagePublisher
{
    Task PublishWithRetryAsync(string queue, byte[] message);
}