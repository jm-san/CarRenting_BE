using Application.Common.Enums;
using Application.Common.Models;
using Application.Costumers.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Costumers.Queries.GetCustomers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, ApiResponse<List<CustomerDto>>>
{
    private readonly IRepository<Customer, CustomerFilter> _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(IRepository<Customer, CustomerFilter> customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var customers = await _customerRepository.GetAllAsync(request.Filters);
            return new ApiResponse<List<CustomerDto>>(ETypeApiResponse.OK, _mapper.Map<List<CustomerDto>>(customers));
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CustomerDto>>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
