using Application.Common.Enums;
using Application.Common.Models;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Costumers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ApiResponse<string>>
{
    private readonly IRepository<Customer, CustomerFilter> _customerRepository;

    public DeleteCustomerCommandHandler(IRepository<Customer, CustomerFilter> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ApiResponse<string>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _customerRepository.DeleteAsync(request.Id);
            return new ApiResponse<string>(ETypeApiResponse.OK, request.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
