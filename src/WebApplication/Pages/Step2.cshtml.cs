using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using PathbitLevel2Challenge.Domain.Models;

namespace PathbitLevel2Challenge.WebApplication.Pages
{
  public class Step2Model : PageModel
  {
    [BindProperty]
    public Customer Customer { get; set; } = new Customer();
    public IActionResult OnGet()
    {
      var customerBytes = HttpContext.Session.Get("Customer");

      if (customerBytes == null)
        return RedirectToPage("/Index"); // Redireciona caso a sessão tenha expirado

      var customerJson = Encoding.UTF8.GetString(customerBytes);
      Customer = JsonSerializer.Deserialize<Customer>(customerJson) ?? new Customer();

      return Page();
    }

    public IActionResult OnPost()
    {
      if (!ModelState.IsValid)
        return Page();

      var customerJson = JsonSerializer.Serialize(Customer);
      HttpContext.Session.Set("Customer", Encoding.UTF8.GetBytes(customerJson));

      return RedirectToPage("/Step3");
    }
  }
}
