
using Microsoft.AspNetCore.Mvc;
using PathbitLevel2Challenge.Application.Commands;
using PathbitLevel2Challenge.Domain.Models;
using MediatR;

namespace PathbitLevel2Challenge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
  private readonly IMediator _mediator;

  public CustomerController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<IActionResult> Register([FromBody] RegisterCustomerCommand command)
  {
    var customerId = await _mediator.Send(command);
    return Ok(new { CustomerId = customerId });
  }
}
