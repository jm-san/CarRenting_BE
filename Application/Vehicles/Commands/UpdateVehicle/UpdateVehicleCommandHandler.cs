using Application.Common.Enums;
using Application.Common.Models;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, ApiResponse<string>>
{
    private readonly IVehicleRepository _vehicleRepository;

    public UpdateVehicleCommandHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<ApiResponse<string>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Id);
            if (vehicle == null)
            {
                return new ApiResponse<string>(ETypeApiResponse.ENTITY_NOT_FOUND, request.Id, "No existe el vehículo con el Id indicado");
            }

            vehicle.Brand = request.Vehicle.Brand ?? vehicle.Brand;
            vehicle.Model = request.Vehicle.Model ?? vehicle.Model;
            vehicle.NumberPlate = request.Vehicle.NumberPlate ?? vehicle.NumberPlate;
            vehicle.ManufacturingDate = request.Vehicle.ManufacturingDate ?? vehicle.ManufacturingDate;

            await _vehicleRepository.UpdateAsync(request.Id, vehicle);

            return new ApiResponse<string>(ETypeApiResponse.OK, vehicle.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
