using Application.Common.Enums;
using Application.Common.Models;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Costumers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, ApiResponse<string>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<UpdateCustomerCommand> _validator;

    public UpdateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IValidator<UpdateCustomerCommand> validator)
    {
        _customerRepository = customerRepository;
        _validator = validator;
    }

    public async Task<ApiResponse<string>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(ETypeApiResponse.VALIDATION_ERROR, validationResult.ToString());
            }

            var customer = await _customerRepository.GetByIdAsync(request.Id);
            if (customer == null)
            {
                return new ApiResponse<string>(ETypeApiResponse.ENTITY_NOT_FOUND, request.Id, "No existe el cliente con el Id indicado");
            }

            customer.Name = request.Customer.Name ?? customer.Name;
            customer.LastName = request.Customer.LastName ?? customer.LastName;
            customer.DNI = request.Customer.DNI ?? customer.DNI;
            customer.Telephone = request.Customer.Telephone ?? customer.Telephone;
            customer.Birthdate = request.Customer.Birthdate ?? customer.Birthdate;

            await _customerRepository.UpdateAsync(request.Id, customer);

            return new ApiResponse<string>(ETypeApiResponse.OK, customer.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
