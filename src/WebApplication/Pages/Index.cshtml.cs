
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PathbitLevel2Challenge.Domain.Models;
using System.Text.Json;
using System.Text;

namespace PathbitLevel2Challenge.WebApplication.Pages;

public class IndexModel : PageModel
{
  [BindProperty]
  public Customer Customer { get; set; } = new Customer();

  public void OnGet()
  {
    Customer = new Customer
    {
      BasicData = new BasicData(),
      FinancialData = new FinancialData(),
      AddressData = new AddressData(),
      SecurityData = new SecurityData()
    };
  }

  public IActionResult OnPost()
  {
    if (!ModelState.IsValid)
      return Page();

   
    HttpContext.Session.SetString("Customer", JsonSerializer.Serialize(Customer));

    return RedirectToPage("/Step2");
  }

}
