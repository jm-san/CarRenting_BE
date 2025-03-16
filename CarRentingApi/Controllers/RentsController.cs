using Application.Common.Enums;
using Application.Common.Models;
using Application.Rents.Dtos;
using Application.Rents.Services;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class RentsController : ControllerBase
{
    public readonly RentService _rentService;

    public RentsController(RentService rentService)
    {
        _rentService = rentService;
    }

    // GET: api/Rents/GetRents
    [HttpGet]
    public async Task<ApiResponse<List<RentDto>>> GetRents([FromQuery] RentFilter filters)
    {
        try
        {
            return await _rentService.GetRents(filters);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<RentDto>>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    // GET: api/Rents/GetRent/{id}
    [HttpGet("{id}")]
    public async Task<ApiResponse<RentDto>> GetRent(string id) =>
        await _rentService.GetRent(id);

    // POST: api/Rents/CreateRent
    [HttpPost]
    public async Task<ApiResponse<string>> CreateRent([FromBody] RentInDto rentDto) =>
        await _rentService.CreateRent(rentDto);

    // PUT: api/Rents/UpdateRentActivity/{id}
    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> UpdateRentActivity(string id, [FromBody] RentInDto rentDto) =>
        await _rentService.UpdateRentActivity(id, rentDto);

    // DELETE: api/Rents/DeleteRent/{id}
    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> DeleteRent(string id) =>
        await _rentService.DeleteRent(id);

}