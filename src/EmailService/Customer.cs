using PathbitLevel2Challenge.Domain.Interfaces;

namespace PathbitLevel2Challenge.EmailService;

public class Customer : IEmailMessage
{
  public string? Id { get; set; }
  public BasicData? BasicData { get; set; }
  public string Name => BasicData?.Name ?? string.Empty;
  public string Email => BasicData?.Email ?? string.Empty;
}

public class BasicData
{
  public string? Name { get; set; }
  public string? Email { get; set; }
}
