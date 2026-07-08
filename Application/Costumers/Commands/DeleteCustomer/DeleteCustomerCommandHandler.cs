using Application.Common.Enums;
using Application.Common.Models;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Costumers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ApiResponse<string>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<DeleteCustomerCommand> _validator;

    public DeleteCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IValidator<DeleteCustomerCommand> validator)
    {
        _customerRepository = customerRepository;
        _validator = validator;
    }

    public async Task<ApiResponse<string>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(ETypeApiResponse.VALIDATION_ERROR, validationResult.ToString());
            }

            await _customerRepository.DeleteAsync(request.Id);
            return new ApiResponse<string>(ETypeApiResponse.OK, request.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
