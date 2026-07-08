using Application.Common.Enums;
using Application.Common.Models;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, ApiResponse<string>>
{
    private readonly IVehicleRepository _vehicleRepository;

    public DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository)
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
