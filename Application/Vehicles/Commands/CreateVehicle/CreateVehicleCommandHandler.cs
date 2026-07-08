using Application.Common.Enums;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, ApiResponse<string>>
{
    private readonly IRepository<Vehicle, VehicleFilter> _vehicleRepository;
    private readonly IMapper _mapper;

    public CreateVehicleCommandHandler(
        IRepository<Vehicle, VehicleFilter> vehicleRepository,
        IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<string>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
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
