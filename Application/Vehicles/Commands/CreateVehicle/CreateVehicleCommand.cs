using Application.Common.Models;
using Application.Vehicles.Dtos;
using MediatR;

namespace Application.Vehicles.Commands.CreateVehicle;

public record CreateVehicleCommand(VehicleInDto Vehicle) : IRequest<ApiResponse<string>>;
