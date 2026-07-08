using Application.Common.Enums;
using Application.Common.Models;
using Application.Vehicles.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Vehicles.Queries.GetVehicle;

public class GetVehicleQueryHandler : IRequestHandler<GetVehicleQuery, ApiResponse<VehicleDto>>
{
    private readonly IRepository<Vehicle, VehicleFilter> _vehicleRepository;
    private readonly IMapper _mapper;

    public GetVehicleQueryHandler(IRepository<Vehicle, VehicleFilter> vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<VehicleDto>> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Id);
            return new ApiResponse<VehicleDto>(ETypeApiResponse.OK, _mapper.Map<VehicleDto>(vehicle));
        }
        catch (Exception ex)
        {
            return new ApiResponse<VehicleDto>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
