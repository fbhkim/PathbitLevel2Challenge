using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using PathbitLevel2Challenge.Domain.Interfaces;

namespace PathbitLevel2Challenge.Infrastructure.Services;

public class MessageQueueService : IMessageQueueService, IDisposable
{
  private readonly IConnection _connection;
  private readonly IModel _channel;

  public MessageQueueService(IConnectionFactory connectionFactory)
  {
    _connection = connectionFactory.CreateConnection();
    _channel = _connection.CreateModel();
  }

  public async Task PublishAsync<T>(string queue, T message)
  {
    _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);

    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
    _channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);

    await Task.CompletedTask;
  }

  public void Consume<T>(string queue, Func<T, Task> onMessage)
  {
    _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);

    var consumer = new EventingBasicConsumer(_channel);
    consumer.Received += async (model, ea) =>
    {
      var body = ea.Body.ToArray();
      var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));
      if (message != null)
      {
        await onMessage(message);
        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
      }
    };

    _channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
  }

  public void Dispose()
  {
    _channel?.Close();
    _connection?.Close();
  }
}
