using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PathbitLevel2Challenge.Application.Commands;
using PathbitLevel2Challenge.Domain.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PathbitLevel2Challenge.WebApplication.Pages;

public class ConfirmationModel : PageModel
{
  private readonly HttpClient _httpClient;

  public ConfirmationModel(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<IActionResult> OnGetAsync()
  {
    var customerJson = HttpContext.Session.GetString("Customer");
    if (string.IsNullOrEmpty(customerJson))
    {
      // Caso não tenha dados, redireciona para a página inicial
      return RedirectToPage("/Index");
    }

    var customer = JsonSerializer.Deserialize<Customer>(customerJson);
    if (customer == null)
    {
      return RedirectToPage("/Index");
    }

    var command = new RegisterCustomerCommand
    {
      BasicData = customer.BasicData,
      FinancialData = customer.FinancialData,
      AddressData = customer.AddressData,
      SecurityData = new SecurityDataInput
      {
        Password = customer.SecurityData?.PasswordHash,
        ConfirmPassword = customer.SecurityData?.PasswordHash
      }
    };

    try
    {
      var response = await _httpClient.PostAsJsonAsync("https://localhost:5001/api/Customer", command);
      if (!response.IsSuccessStatusCode)
      {
        ModelState.AddModelError(string.Empty, "Erro ao enviar dados do cliente.");
        return Page();
      }
    }
    catch (HttpRequestException)
    {
      ModelState.AddModelError(string.Empty, "Não foi possível conectar ao serviço. Tente novamente mais tarde.");
      return Page();
    }

    HttpContext.Session.Remove("Customer");
    return Page();
  }
}
