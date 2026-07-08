using Application.Common.Models;
using Application.Vehicles.Dtos;
using Domain.Filters;
using MediatR;

namespace Application.Vehicles.Queries.GetVehicles;

public record GetVehiclesQuery(VehicleFilter Filters) : IRequest<ApiResponse<List<VehicleDto>>>;
