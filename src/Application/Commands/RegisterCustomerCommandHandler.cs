
using MediatR;
using PathbitLevel2Challenge.Common.Helpers;
using PathbitLevel2Challenge.Domain.Interfaces;
using PathbitLevel2Challenge.Domain.Models;

namespace PathbitLevel2Challenge.Application.Commands;

public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, string>
{
  private readonly ICustomerRepository _customerRepository;
  private readonly IMessageQueueService _messageQueueService;
  private readonly ICepService _cepService;

  public RegisterCustomerCommandHandler(
      ICustomerRepository customerRepository,
      IMessageQueueService messageQueueService,
      ICepService cepService)
  {
    _customerRepository = customerRepository;
    _messageQueueService = messageQueueService;
    _cepService = cepService;
  }

  public async Task<string> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
  {
    // Garantir que AddressData e CEP não são nulos
    if (request.AddressData?.CEP is null)
      throw new ArgumentNullException(nameof(request.AddressData.CEP), "CEP é obrigatório.");

    var cepData = await _cepService.GetAddressAsync(request.AddressData.CEP);
    if (cepData == null)
      throw new Exception("CEP inválido.");

    request.AddressData.Street = cepData.Street;
    request.AddressData.Neighborhood = cepData.Neighborhood;
    request.AddressData.City = cepData.City;
    request.AddressData.State = cepData.State;

    // Validar SecurityData
    if (string.IsNullOrWhiteSpace(request.SecurityData?.Password))
      throw new ArgumentNullException(nameof(request.SecurityData.Password), "Senha é obrigatória.");

    // Validar dados restantes
    if (request.BasicData is null)
      throw new ArgumentNullException(nameof(request.BasicData), "Dados básicos são obrigatórios.");

    if (request.FinancialData is null)
      throw new ArgumentNullException(nameof(request.FinancialData), "Dados financeiros são obrigatórios.");

    // Criar o cliente
    var customer = new Customer
    {
      Id = Guid.NewGuid().ToString(),
      BasicData = request.BasicData,
      FinancialData = request.FinancialData,
      AddressData = request.AddressData,
      SecurityData = new SecurityData
      {
        PasswordHash = PasswordHasher.HashPassword(request.SecurityData.Password)
      },
      CreatedAt = DateTime.UtcNow
    };

    // Salvar no MongoDB
    await _customerRepository.AddAsync(customer);

    // Enviar mensagem para a fila
    await _messageQueueService.PublishAsync("cadastro.em.analise.email", customer);

    return customer.Id;
  }

}
