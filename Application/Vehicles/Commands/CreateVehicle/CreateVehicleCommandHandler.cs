using Application.Common.Enums;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, ApiResponse<string>>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateVehicleCommand> _validator;

    public CreateVehicleCommandHandler(
        IVehicleRepository vehicleRepository,
        IMapper mapper,
        IValidator<CreateVehicleCommand> validator)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ApiResponse<string>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(ETypeApiResponse.VALIDATION_ERROR, validationResult.ToString());
            }

            var vehicle = _mapper.Map<Vehicle>(request.Vehicle);

            await _vehicleRepository.InsertAsync(vehicle);
            return new ApiResponse<string>(ETypeApiResponse.OK, vehicle.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
