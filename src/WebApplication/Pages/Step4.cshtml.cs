using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PathbitLevel2Challenge.Application.Commands;
using PathbitLevel2Challenge.Domain.Models;
using System.Text;
using System.Text.Json;

namespace PathbitLevel2Challenge.WebApplication.Pages;

public class Step4Model : PageModel
{
  [BindProperty]
  public SecurityDataInput SecurityData { get; set; } = new SecurityDataInput();

  public void OnGet()
  {
    // Inicializa a propriedade para evitar null
    SecurityData = new SecurityDataInput();
  }

  public IActionResult OnPost()
  {
    if (!ModelState.IsValid)
      return Page();

    var customerJson = HttpContext.Session.GetString("Customer");
    var customer = customerJson != null
        ? JsonSerializer.Deserialize<Customer>(customerJson) ?? new Customer()
        : new Customer();

    // A senha será hasheada na API, aqui fica temporário
    customer.SecurityData = new SecurityData { PasswordHash = SecurityData.Password };

    var updatedJson = JsonSerializer.Serialize(customer);
    var updatedBytes = Encoding.UTF8.GetBytes(updatedJson);
    HttpContext.Session.Set("Customer", updatedBytes);

    return RedirectToPage("/Confirmation");
  }
}
