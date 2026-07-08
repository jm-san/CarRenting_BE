using Application.Common.Models;
using Application.Vehicles.Dtos;
using MediatR;

namespace Application.Vehicles.Commands.UpdateVehicle;

public record UpdateVehicleCommand(string Id, UpdateVehicleDto Vehicle) : IRequest<ApiResponse<string>>;
