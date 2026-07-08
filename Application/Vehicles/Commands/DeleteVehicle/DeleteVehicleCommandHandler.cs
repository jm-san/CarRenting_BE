using Application.Common.Enums;
using Application.Common.Models;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, ApiResponse<string>>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IValidator<DeleteVehicleCommand> _validator;

    public DeleteVehicleCommandHandler(
        IVehicleRepository vehicleRepository,
        IValidator<DeleteVehicleCommand> validator)
    {
        _vehicleRepository = vehicleRepository;
        _validator = validator;
    }

    public async Task<ApiResponse<string>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(ETypeApiResponse.VALIDATION_ERROR, validationResult.ToString());
            }

            await _vehicleRepository.DeleteAsync(request.Id);
            return new ApiResponse<string>(ETypeApiResponse.OK, request.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
