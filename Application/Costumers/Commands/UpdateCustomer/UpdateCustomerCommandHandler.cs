using Application.Common.Enums;
using Application.Common.Models;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Costumers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, ApiResponse<string>>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ApiResponse<string>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
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
