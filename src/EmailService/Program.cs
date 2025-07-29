using Microsoft.Extensions.Configuration;
using PathbitLevel2Challenge.Infrastructure.Services;
using PathbitLevel2Challenge.Domain.Models;
using SendGrid;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace PathbitLevel2Challenge.EmailService;

class Program
{
  static async Task Main(string[] args)
  {
    // Carrega configuração
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    // Cria conexão com RabbitMQ
    var rabbitMqConnectionString = configuration["RabbitMQConnectionString"];
    if (string.IsNullOrEmpty(rabbitMqConnectionString))
    {
      Console.WriteLine("RabbitMQConnectionString não configurada.");
      return;
    }

    var factory = new ConnectionFactory
    {
      Uri = new Uri(rabbitMqConnectionString)
    };

    var messageQueueService = new MessageQueueService(factory);

    // Cria client do SendGrid
    var sendGridApiKey = configuration["SendGridApiKey"];
    if (string.IsNullOrEmpty(sendGridApiKey))
    {
      Console.WriteLine("SendGridApiKey não configurada.");
      return;
    }

    var sendGridClient = new SendGridClient(sendGridApiKey);
    var emailSenderService = new EmailSenderService(sendGridClient);

    // Consome fila
    messageQueueService.Consume<Customer>("cadastro.em.analise.email", async (customer) =>
    {
      await emailSenderService.SendEmailAsync(customer);
    });

    Console.WriteLine("✅ Serviço de envio de e-mail iniciado. Aguardando mensagens...");
    await Task.Delay(Timeout.Infinite);
  }
}
