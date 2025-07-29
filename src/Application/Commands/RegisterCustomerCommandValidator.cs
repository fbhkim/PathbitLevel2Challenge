using FluentValidation;
using PathbitLevel2Challenge.Common.Validators;

namespace PathbitLevel2Challenge.Application.Commands
{
  public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
  {
    public RegisterCustomerCommandValidator()
    {

      RuleFor(x => x.BasicData)
          .NotNull().WithMessage("Dados básicos são obrigatórios.")
          .DependentRules(() =>
          {
            RuleFor(x => x.BasicData!.Name)
                      .NotEmpty().WithMessage("Nome é obrigatório.")
                      .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

            RuleFor(x => x.BasicData!.BirthDate)
                      .Must(BeOver18).WithMessage("Cliente deve ser maior de idade (18 anos).");

            RuleFor(x => x.BasicData!.CPF)
                      .NotEmpty().WithMessage("CPF é obrigatório.")
                      .Must(cpf => cpf != null && CpfValidator.IsValid(cpf))
                      .WithMessage("CPF inválido.");

            RuleFor(x => x.BasicData!.Email)
                      .NotEmpty().WithMessage("Email é obrigatório.")
                      .EmailAddress().WithMessage("Email inválido.");

            RuleFor(x => x.BasicData!.Phone)
                      .NotEmpty().WithMessage("Telefone é obrigatório.")
                      .Matches(@"^\(\d{2}\)\s\d{4,5}-\d{4}$").WithMessage("Telefone deve estar no formato (XX) XXXXX-XXXX.");
          });


      RuleFor(x => x.FinancialData)
          .NotNull().WithMessage("Dados financeiros são obrigatórios.")
          .DependentRules(() =>
          {
            RuleFor(x => x.FinancialData!.Income + x.FinancialData!.Assets)
                      .GreaterThan(1000).WithMessage("Soma de renda e patrimônio deve ser maior que 1000.");
          });


      RuleFor(x => x.AddressData)
          .NotNull().WithMessage("Endereço é obrigatório.")
          .DependentRules(() =>
          {
            RuleFor(x => x.AddressData!.CEP)
                      .NotEmpty().WithMessage("CEP é obrigatório.")
                      .Matches(@"^\d{5}-\d{3}$").WithMessage("CEP deve estar no formato XXXXX-XXX.");
          });


      RuleFor(x => x.SecurityData)
          .NotNull().WithMessage("Dados de segurança são obrigatórios.")
          .DependentRules(() =>
          {
            RuleFor(x => x.SecurityData!.Password)
                      .NotEmpty().WithMessage("Senha é obrigatória.")
                      .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres.");

            RuleFor(x => x.SecurityData!.ConfirmPassword)
                      .Equal(x => x.SecurityData!.Password)
                      .WithMessage("Confirmação de senha não coincide.");
          });
    }

    private bool BeOver18(DateTime birthDate)
    {
      return birthDate.AddYears(18) <= DateTime.Now;
    }
  }
}
