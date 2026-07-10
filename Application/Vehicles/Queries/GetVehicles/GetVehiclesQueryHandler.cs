using Application.Common.Enums;
using Application.Common.Models;
using Application.Vehicles.Dtos;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Vehicles.Queries.GetVehicles;

public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, ApiResponse<List<VehicleDto>>>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;

    public GetVehiclesQueryHandler(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<VehicleDto>>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicles = await _vehicleRepository.GetAllAsync(request.Filters);
            return new ApiResponse<List<VehicleDto>>(ETypeApiResponse.OK, _mapper.Map<List<VehicleDto>>(vehicles));
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<VehicleDto>>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
