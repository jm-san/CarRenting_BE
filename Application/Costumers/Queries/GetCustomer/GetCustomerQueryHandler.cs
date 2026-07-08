using Application.Common.Enums;
using Application.Common.Models;
using Application.Costumers.Dtos;
using AutoMapper;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Costumers.Queries.GetCustomer;

public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, ApiResponse<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<GetCustomerQuery> _validator;

    public GetCustomerQueryHandler(
        ICustomerRepository customerRepository,
        IMapper mapper,
        IValidator<GetCustomerQuery> validator)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ApiResponse<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ApiResponse<CustomerDto>(ETypeApiResponse.VALIDATION_ERROR, validationResult.ToString());
            }

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
