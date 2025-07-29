
using MongoDB.Driver;
using PathbitLevel2Challenge.Domain.Interfaces;
using PathbitLevel2Challenge.Domain.Models;

namespace PathbitLevel2Challenge.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
  private readonly IMongoCollection<Customer> _customers;

  public CustomerRepository(IMongoClient mongoClient)
  {
    var database = mongoClient.GetDatabase("Pathbit");
    _customers = database.GetCollection<Customer>("Customers");
  }

  public async Task AddAsync(Customer customer)
  {
    await _customers.InsertOneAsync(customer);
  }
}
