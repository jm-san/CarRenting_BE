using Application.Common.Enums;
using Application.Common.Models;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Rents.Commands.DeleteRent;

public class DeleteRentCommandHandler : IRequestHandler<DeleteRentCommand, ApiResponse<string>>
{
    private readonly IRepository<Rent, RentFilter> _rentRepository;

    public DeleteRentCommandHandler(IRepository<Rent, RentFilter> rentRepository)
    {
        _rentRepository = rentRepository;
    }

    public async Task<ApiResponse<string>> Handle(DeleteRentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _rentRepository.DeleteAsync(request.Id);
            return new ApiResponse<string>(ETypeApiResponse.OK, request.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
