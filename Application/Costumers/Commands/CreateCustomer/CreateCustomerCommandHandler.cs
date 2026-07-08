using Application.Common.Enums;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Costumers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ApiResponse<string>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCustomerCommand> _validator;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IMapper mapper,
        IValidator<CreateCustomerCommand> validator)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ApiResponse<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(ETypeApiResponse.VALIDATION_ERROR, validationResult.ToString());
            }

            var customer = _mapper.Map<Customer>(request.Customer);
            await _customerRepository.InsertAsync(customer);

            return new ApiResponse<string>(ETypeApiResponse.OK, customer.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
