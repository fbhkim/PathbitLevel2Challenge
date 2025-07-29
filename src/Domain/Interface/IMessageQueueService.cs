namespace PathbitLevel2Challenge.Domain.Interfaces;

public interface IMessageQueueService : IDisposable
{
  Task PublishAsync<T>(string queue, T message);
  void Consume<T>(string queue, Func<T, Task> onMessage);
}
