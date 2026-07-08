using Application.Common.Models;
using Application.Vehicles.Dtos;
using MediatR;

namespace Application.Vehicles.Queries.GetVehicle;

public record GetVehicleQuery(string Id) : IRequest<ApiResponse<VehicleDto>>;
