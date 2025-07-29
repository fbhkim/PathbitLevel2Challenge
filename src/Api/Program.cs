using MediatR;
using MongoDB.Driver;
using PathbitLevel2Challenge.Application.Commands;
using PathbitLevel2Challenge.Domain.Interfaces;
using PathbitLevel2Challenge.Infrastructure.Repositories;
using PathbitLevel2Challenge.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using RabbitMQ.Client;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterCustomerCommand).Assembly));

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration["MongoDBConnectionString"]));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddHttpClient<ICepService, CepService>();

builder.Services.AddSingleton<IMessageQueueService>(sp =>
{
  var connectionString = builder.Configuration["RabbitMQConnectionString"];
  if (string.IsNullOrEmpty(connectionString))
  {
    throw new InvalidOperationException("RabbitMQConnectionString não está configurada no appsettings.json");
  }

  var factory = new ConnectionFactory
  {
    Uri = new Uri(connectionString),
  };

  return new MessageQueueService(factory);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pathbit API V1");
  });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
