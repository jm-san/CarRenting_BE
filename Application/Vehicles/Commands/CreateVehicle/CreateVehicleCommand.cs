using Application.Common.Models;
using Application.Vehicles.Dtos;
using MediatR;

namespace Application.Vehicles.Commands.CreateVehicle;

public record CreateVehicleCommand(CreateVehicleDto Vehicle) : IRequest<ApiResponse<string>>;
