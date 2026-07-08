using Application.Common.Enums;
using Application.Common.Models;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, ApiResponse<string>>
{
    private readonly IRepository<Vehicle, VehicleFilter> _vehicleRepository;

    public DeleteVehicleCommandHandler(IRepository<Vehicle, VehicleFilter> vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<ApiResponse<string>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _vehicleRepository.DeleteAsync(request.Id);
            return new ApiResponse<string>(ETypeApiResponse.OK, request.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
