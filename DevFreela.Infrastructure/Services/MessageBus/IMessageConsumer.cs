namespace DevFreela.Infrastructure.Services.MessageBus;

public interface IMessageConsumer
{
    void Consume(string queue);
}