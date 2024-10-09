namespace DevFreela.Infrastructure.Services.MessageBus.Publisher;

public interface IMessagePublisher
{
    Task PublishWithRetryAsync(string queue, byte[] message);
}