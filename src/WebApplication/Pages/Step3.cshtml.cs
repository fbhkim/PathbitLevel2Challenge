using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PathbitLevel2Challenge.Domain.Models;
using System.Text;
using System.Text.Json;

namespace PathbitLevel2Challenge.WebApplication.Pages;

public class Step3Model : PageModel
{
  [BindProperty]
  public Customer Customer { get; set; } = new Customer();

  public void OnGet()
  {
    var customerJson = HttpContext.Session.GetString("Customer");
    Customer = customerJson != null
        ? JsonSerializer.Deserialize<Customer>(customerJson) ?? new Customer()
        : new Customer();
  }

  public IActionResult OnPost()
  {
    if (!ModelState.IsValid)
      return Page();

    var customerJson = HttpContext.Session.GetString("Customer");
    var existingCustomer = customerJson != null
        ? JsonSerializer.Deserialize<Customer>(customerJson) ?? new Customer()
        : new Customer();

    existingCustomer.AddressData = Customer.AddressData;

    var updatedJson = JsonSerializer.Serialize(existingCustomer);
    var updatedBytes = Encoding.UTF8.GetBytes(updatedJson);
    HttpContext.Session.Set("Customer", updatedBytes);

    return RedirectToPage("/Step4");
  }
}
