namespace PathbitLevel2Challenge.Common.Validators;

public static class CpfValidator
{
  public static bool IsValid(string cpf)
  {
    cpf = cpf.Replace(".", "").Replace("-", "");
    if (cpf.Length != 11 || !cpf.All(char.IsDigit)) return false;

    int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
    int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

    string tempCpf = cpf.Substring(0, 9);
    int sum = 0;

    for (int i = 0; i < 9; i++)
      sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

    int remainder = sum % 11;
    if (remainder < 2) remainder = 0; else remainder = 11 - remainder;

    string digit = remainder.ToString();
    tempCpf += digit;
    sum = 0;

    for (int i = 0; i < 10; i++)
      sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

    remainder = sum % 11;
    if (remainder < 2) remainder = 0; else remainder = 11 - remainder;

    digit += remainder.ToString();
    return cpf.EndsWith(digit);
  }
}
