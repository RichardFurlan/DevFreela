namespace DevFreela.Infrastructure.Services.MessageBus;

public interface IMessageBusService
{
    void Publish(string queue, byte[] message);
}