using Application.Rents.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Rents.MappingProfiles;

public class RentMappingProfile : Profile
{
    public RentMappingProfile()
    {
        CreateMap<Rent, RentDto>().ReverseMap();
        CreateMap<RentInDto, Rent>();
    }
}
