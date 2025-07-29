using System;
using System.Threading.Tasks;
using PathbitLevel2Challenge.Domain.Models;

namespace PathbitLevel2Challenge.Domain.Interfaces;

public interface ICustomerRepository
{
  Task AddAsync(Customer customer);
}
