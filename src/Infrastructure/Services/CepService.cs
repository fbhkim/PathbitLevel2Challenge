
using PathbitLevel2Challenge.Domain.Interfaces;
using PathbitLevel2Challenge.Domain.Models;
using System.Net.Http.Json;

namespace PathbitLevel2Challenge.Infrastructure.Services;

public class CepService : ICepService
{
  private readonly HttpClient _httpClient;

  public CepService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<AddressData> GetAddressAsync(string cep)
  {
    cep = cep.Replace("-", "");
    var response = await _httpClient.GetFromJsonAsync<ViaCepResponse>($"https://viacep.com.br/ws/{cep}/json/");

    if (response == null || response.Erro)
      throw new Exception("CEP inválido.");

    return new AddressData
    {
      Street = response.Logradouro,
      Neighborhood = response.Bairro,
      City = response.Localidade,
      State = response.Uf,
      CEP = cep.Insert(5, "-")
    };
  }

}

public class ViaCepResponse
{
  public string? Logradouro { get; set; }
  public string? Bairro { get; set; }
  public string? Localidade { get; set; }
  public string? Uf { get; set; }
  public bool Erro { get; set; }
}
