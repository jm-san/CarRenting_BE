using Application.Common.Enums;
using Application.Common.Models;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, ApiResponse<int>>
{
    private readonly IVehicleRepository _vehicleRepository;

    public DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<ApiResponse<int>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _vehicleRepository.DeleteAsync(request.Id);
            return new ApiResponse<int>(ETypeApiResponse.OK, request.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<int>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
