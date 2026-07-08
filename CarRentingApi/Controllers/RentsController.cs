using Application.Common.Models;
using Application.Rents.Commands.CreateRent;
using Application.Rents.Commands.DeleteRent;
using Application.Rents.Commands.UpdateRentActivity;
using Application.Rents.Dtos;
using Application.Rents.Queries.GetRent;
using Application.Rents.Queries.GetRents;
using Domain.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class RentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/Rents/GetRents
    [HttpGet]
    public async Task<ApiResponse<List<RentDto>>> GetRents([FromQuery] RentFilter filters) =>
        await _mediator.Send(new GetRentsQuery(filters));

    // GET: api/Rents/GetRent/{id}
    [HttpGet("{id}")]
    public async Task<ApiResponse<RentDto>> GetRent(string id) =>
        await _mediator.Send(new GetRentQuery(id));

    // POST: api/Rents/CreateRent
    [HttpPost]
    public async Task<ApiResponse<string>> CreateRent([FromBody] CreateRentDto rentDto) =>
        await _mediator.Send(new CreateRentCommand(rentDto));

    // PUT: api/Rents/UpdateRentActivity/{id}
    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> UpdateRentActivity(string id, [FromBody] bool isActive) =>
        await _mediator.Send(new UpdateRentActivityCommand(id, isActive));

    // DELETE: api/Rents/DeleteRent/{id}
    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> DeleteRent(string id) =>
        await _mediator.Send(new DeleteRentCommand(id));

}