
using FluentAssertions;
using PathbitLevel2Challenge.Application.Commands;
using PathbitLevel2Challenge.Domain.Models;
using Xunit;

namespace PathbitLevel2Challenge.Tests;

public class RegisterCustomerCommandValidatorTests
{
  private readonly RegisterCustomerCommandValidator _validator;

  public RegisterCustomerCommandValidatorTests()
  {
    _validator = new RegisterCustomerCommandValidator();
  }

  [Fact]
  public void Validate_ValidCommand_ShouldBeValid()
  {
    var command = new RegisterCustomerCommand
    {
      BasicData = new BasicData
      {
        Name = "João Silva",
        BirthDate = DateTime.Now.AddYears(-20),
        CPF = "12345678909",
        Email = "joao@example.com",
        Phone = "(11) 91234-5678"
      },
      FinancialData = new FinancialData { Income = 1000, Assets = 500 },
      AddressData = new AddressData { CEP = "12345-678", Number = "123" },
      SecurityData = new SecurityDataInput { Password = "Password123", ConfirmPassword = "Password123" }
    };

    var result = _validator.Validate(command);
    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_InvalidCPF_ShouldBeInvalid()
  {
    var command = new RegisterCustomerCommand
    {
      BasicData = new BasicData
      {
        Name = "João Silva",
        BirthDate = DateTime.Now.AddYears(-20),
        CPF = "12345678900",
        Email = "joao@example.com",
        Phone = "(11) 91234-5678"
      },
      FinancialData = new FinancialData { Income = 1000, Assets = 500 },
      AddressData = new AddressData { CEP = "12345-678", Number = "123" },
      SecurityData = new SecurityDataInput { Password = "Password123", ConfirmPassword = "Password123" }
    };

    var result = _validator.Validate(command);
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == "CPF inválido.");
  }
}
