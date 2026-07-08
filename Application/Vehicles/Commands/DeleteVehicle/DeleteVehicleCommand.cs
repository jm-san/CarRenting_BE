using Application.Common.Models;
using MediatR;

namespace Application.Vehicles.Commands.DeleteVehicle;

public record DeleteVehicleCommand(string Id) : IRequest<ApiResponse<string>>;
