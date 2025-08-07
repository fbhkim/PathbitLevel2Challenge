
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PathbitLevel2Challenge.WebApplication;

public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddRazorPages();
    services.AddHttpClient();
    services.AddSession();
  }

  public void Configure(IApplicationBuilder app)
  {
    app.UseSession();
   
  }
}
