using Application.Common.Enums;
using Application.Common.Models;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Rents.Commands.UpdateRentActivity;

public class UpdateRentActivityCommandHandler : IRequestHandler<UpdateRentActivityCommand, ApiResponse<string>>
{
    private readonly IRepository<Rent, RentFilter> _rentRepository;

    public UpdateRentActivityCommandHandler(IRepository<Rent, RentFilter> rentRepository)
    {
        _rentRepository = rentRepository;
    }

    public async Task<ApiResponse<string>> Handle(UpdateRentActivityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var rent = await _rentRepository.GetByIdAsync(request.Id);
            if (rent == null)
            {
                return new ApiResponse<string>(ETypeApiResponse.ENTITY_NOT_FOUND, request.Id, "No existe un alquiler con el Id indicado");
            }

            rent.IsActive = request.IsActive;

            await _rentRepository.UpdateAsync(request.Id, rent);
            return new ApiResponse<string>(ETypeApiResponse.OK, rent.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
