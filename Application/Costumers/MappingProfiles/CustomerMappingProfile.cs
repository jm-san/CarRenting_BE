using Application.Costumers.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Costumers.MappingProfiles;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<CustomerInDto, Customer>();
    }
}
