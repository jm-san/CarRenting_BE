using Application.Common.Enums;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Costumers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ApiResponse<int>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = _mapper.Map<Customer>(request.Customer);
            await _customerRepository.InsertAsync(customer);

            return new ApiResponse<int>(ETypeApiResponse.OK, customer.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<int>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
