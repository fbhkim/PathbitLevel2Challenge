
namespace PathbitLevel2Challenge.Domain.Models;

public class Customer
{
  public string? Id { get; set; }
  public BasicData? BasicData { get; set; }
  public FinancialData? FinancialData { get; set; }
  public AddressData? AddressData { get; set; }
  public SecurityData? SecurityData { get; set; }
  public DateTime CreatedAt { get; set; }
}

public class BasicData
{
  public string? Name { get; set; }
  public DateTime BirthDate { get; set; }
  public string? CPF { get; set; }
  public string? Email { get; set; }
  public string? Phone { get; set; }
}

public class FinancialData
{
  public decimal Income { get; set; }
  public decimal Assets { get; set; }
}

public class AddressData
{
  public string? Street { get; set; }
  public string? Number { get; set; }
  public string? Neighborhood { get; set; }
  public string? City { get; set; }
  public string? State { get; set; }
  public string? CEP { get; set; }
}

public class SecurityData
{
  public string? PasswordHash { get; set; }
}
