using Application.Vehicles.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Vehicles.MappingProfiles;

public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        CreateMap<Vehicle, VehicleDto>().ReverseMap();
        CreateMap<VehicleInDto, Vehicle>();
    }
}
