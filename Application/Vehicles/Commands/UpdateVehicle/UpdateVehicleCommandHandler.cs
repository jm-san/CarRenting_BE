using Application.Common.Enums;
using Application.Common.Models;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, ApiResponse<string>>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IValidator<UpdateVehicleCommand> _validator;

    public UpdateVehicleCommandHandler(
        IVehicleRepository vehicleRepository,
        IValidator<UpdateVehicleCommand> validator)
    {
        _vehicleRepository = vehicleRepository;
        _validator = validator;
    }

    public async Task<ApiResponse<string>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(ETypeApiResponse.VALIDATION_ERROR, validationResult.ToString());
            }

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
