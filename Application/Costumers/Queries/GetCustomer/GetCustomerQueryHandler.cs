using Application.Common.Enums;
using Application.Common.Models;
using Application.Costumers.Dtos;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Costumers.Queries.GetCustomer;

public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, ApiResponse<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerQueryHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);
            if (customer == null)
            {
                return new ApiResponse<CustomerDto>(ETypeApiResponse.ENTITY_NOT_FOUND, "No existe el cliente con el Id indicado");
            }

            return new ApiResponse<CustomerDto>(ETypeApiResponse.OK, _mapper.Map<CustomerDto>(customer));
        }
        catch (Exception ex)
        {
            return new ApiResponse<CustomerDto>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
