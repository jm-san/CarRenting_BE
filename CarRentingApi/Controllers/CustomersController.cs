using Application.Common.Models;
using Application.Costumers.Commands.CreateCustomer;
using Application.Costumers.Commands.DeleteCustomer;
using Application.Costumers.Commands.UpdateCustomer;
using Application.Costumers.Dtos;
using Application.Costumers.Queries.GetCustomer;
using Application.Costumers.Queries.GetCustomers;
using Domain.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/Customers/GetCustomers
    [HttpGet]
    public async Task<ApiResponse<List<CustomerDto>>> GetCustomers([FromQuery] CustomerFilter filters) =>
        await _mediator.Send(new GetCustomersQuery(filters));

    // GET: api/Customers/GetCustomer/{id}
    [HttpGet("{id}")]
    public async Task<ApiResponse<CustomerDto>> GetCustomer(string id) =>
        await _mediator.Send(new GetCustomerQuery(id));

    // POST: api/Customers/CreateCustomer
    [HttpPost]
    public async Task<ApiResponse<string>> CreateCustomer([FromBody] CreateCustomerDto customerDto) =>
        await _mediator.Send(new CreateCustomerCommand(customerDto));

    // PUT: api/Customers/UpdateCustomer/{id}
    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> UpdateCustomer(string id, [FromBody] UpdateCustomerDto customerDto) =>
        await _mediator.Send(new UpdateCustomerCommand(id, customerDto));

    // DELETE: api/Customers/DeleteCustomer/{id}
    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> DeleteCustomer(string id) =>
        await _mediator.Send(new DeleteCustomerCommand(id));

}
