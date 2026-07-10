using Application.Common.Models;
using Application.Vehicles.Dtos;
using MediatR;

namespace Application.Vehicles.Queries.GetVehicle;

public record GetVehicleQuery(int Id) : IRequest<ApiResponse<VehicleDto>>;
