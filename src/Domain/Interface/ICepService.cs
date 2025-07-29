using System;
using System.Threading.Tasks;
using PathbitLevel2Challenge.Domain.Models;


namespace PathbitLevel2Challenge.Domain.Interfaces;

public interface ICepService
{
  Task<AddressData> GetAddressAsync(string cep);
}
