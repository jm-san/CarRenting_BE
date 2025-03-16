using Application.Common.Enums;
using Application.Common.Models;
using Application.Vehicles.Dtos;
using Application.Vehicles.Services;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
public class VehiclesController : ControllerBase
{
    private readonly VehicleService _vehicleService;

    public VehiclesController(VehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    // GET: api/Vehicles/GetVehicles
    [HttpGet]
    public async Task<ApiResponse<List<VehicleDto>>> GetVehicles([FromQuery] VehicleFilter filters)
    {
        try
        {
            return await _vehicleService.GetVehicles(filters);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<VehicleDto>>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    // GET: api/Vehicles/GetVehicle/{id}
    [HttpGet("{id}")]
    public async Task<ApiResponse<VehicleDto>> GetVehicle(string id) =>
        await _vehicleService.GetVehicle(id);

    // POST: api/Vehicles/CreateVehicle
    [HttpPost]
    public async Task<ApiResponse<string>> CreateVehicle([FromBody] VehicleInDto vehicleDto) =>
        await _vehicleService.CreateVehicle(vehicleDto);

    // PUT: api/Vehicles/UpdateVehicle/{id}
    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> UpdateVehicle(string id, [FromBody] VehicleInDto vehicleDto) =>
        await _vehicleService.UpdateVehicle(id, vehicleDto);

    // DELETE: api/Vehicles/DeleteVehicle/{id}
    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> DeleteVehicle(string id) =>
        await _vehicleService.DeleteVehicle(id);

}