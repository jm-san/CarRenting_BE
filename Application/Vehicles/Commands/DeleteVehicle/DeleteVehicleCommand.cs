using Application.Common.Models;
using MediatR;

namespace Application.Vehicles.Commands.DeleteVehicle;

public record DeleteVehicleCommand(int Id) : IRequest<ApiResponse<int>>;
