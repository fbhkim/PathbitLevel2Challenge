using FluentAssertions;
using Moq;
using MongoDB.Driver;
using PathbitLevel2Challenge.Domain.Models;
using PathbitLevel2Challenge.Infrastructure.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace PathbitLevel2Challenge.Tests
{
  public class CustomerRepositoryTests
  {
    private readonly Mock<IMongoClient> _mongoClientMock;
    private readonly Mock<IMongoDatabase> _databaseMock;
    private readonly Mock<IMongoCollection<Customer>> _collectionMock;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests()
    {
      _mongoClientMock = new Mock<IMongoClient>();
      _databaseMock = new Mock<IMongoDatabase>();
      _collectionMock = new Mock<IMongoCollection<Customer>>();


      _mongoClientMock
          .Setup(client => client.GetDatabase(It.IsAny<string>(), null))
          .Returns(_databaseMock.Object);


      _databaseMock
          .Setup(db => db.GetCollection<Customer>(It.IsAny<string>(), null))
          .Returns(_collectionMock.Object);


      _repository = new CustomerRepository(_mongoClientMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldInsertCustomer()
    {
      // Arrange
      var customer = new Customer
      {
        Id = "1",
        BasicData = new BasicData { Name = "Teste" }
      };

      // Act
      await _repository.AddAsync(customer);

      // Assert
      _collectionMock.Verify(x => x.InsertOneAsync(
          customer,
          It.IsAny<InsertOneOptions>(),
          It.IsAny<System.Threading.CancellationToken>()),
          Times.Once);
    }
  }
}
