using Application.Common.Models;
using Application.Vehicles.Commands.CreateVehicle;
using Application.Vehicles.Commands.DeleteVehicle;
using Application.Vehicles.Commands.UpdateVehicle;
using Application.Vehicles.Dtos;
using Application.Vehicles.Queries.GetVehicle;
using Application.Vehicles.Queries.GetVehicles;
using Domain.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/Vehicles/GetVehicles
    [HttpGet]
    public async Task<ApiResponse<List<VehicleDto>>> GetVehicles([FromQuery] VehicleFilter filters) =>
        await _mediator.Send(new GetVehiclesQuery(filters));

    // GET: api/Vehicles/GetVehicle/{id}
    [HttpGet("{id}")]
    public async Task<ApiResponse<VehicleDto>> GetVehicle(string id) =>
        await _mediator.Send(new GetVehicleQuery(id));

    // POST: api/Vehicles/CreateVehicle
    [HttpPost]
    public async Task<ApiResponse<string>> CreateVehicle([FromBody] CreateVehicleDto vehicleDto) =>
        await _mediator.Send(new CreateVehicleCommand(vehicleDto));

    // PUT: api/Vehicles/UpdateVehicle/{id}
    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> UpdateVehicle(string id, [FromBody] UpdateVehicleDto vehicleDto) =>
        await _mediator.Send(new UpdateVehicleCommand(id, vehicleDto));

    // DELETE: api/Vehicles/DeleteVehicle/{id}
    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> DeleteVehicle(string id) =>
        await _mediator.Send(new DeleteVehicleCommand(id));

}
