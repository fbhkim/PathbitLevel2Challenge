
using MediatR;
using PathbitLevel2Challenge.Domain.Models;

namespace PathbitLevel2Challenge.Application.Commands;

public class RegisterCustomerCommand : IRequest<string>
{
  public BasicData? BasicData { get; set; }
  public FinancialData? FinancialData { get; set; }
  public AddressData? AddressData { get; set; }
  public SecurityDataInput? SecurityData { get; set; }
}

public class SecurityDataInput
{
  public string? Password { get; set; }
  public string? ConfirmPassword { get; set; }
}
