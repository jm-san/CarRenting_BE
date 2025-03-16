using Application.Common.Enums;
using Application.Common.Models;
using Application.Costumers.Dtos;
using Application.Costumers.Services;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: api/Customers/GetCustomers
    [HttpGet]
    public async Task<ApiResponse<List<CustomerDto>>> GetCustomers([FromQuery] CustomerFilter filters)
    {
        try
        {
            return await _customerService.GetCustomers(filters);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CustomerDto>>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    // GET: api/Customers/GetCustomer/{id}
    [HttpGet("{id}")]
    public async Task<ApiResponse<CustomerDto>> GetCustomer(string id) =>
        await _customerService.GetCustomer(id);

    // POST: api/Customers/CreateCustomer
    [HttpPost]
    public async Task<ApiResponse<string>> CreateCustomer([FromBody] CustomerInDto customerDto) =>
        await _customerService.CreateCustomer(customerDto);

    // PUT: api/Customers/UpdateCustomer/{id}
    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> UpdateCustomer(string id, [FromBody] CustomerInDto customerDto) =>
        await _customerService.UpdateCustomer(id, customerDto);

    // DELETE: api/Customers/DeleteCustomer/{id}
    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> DeleteCustomer(string id) =>
        await _customerService.DeleteCustomer(id);

}
