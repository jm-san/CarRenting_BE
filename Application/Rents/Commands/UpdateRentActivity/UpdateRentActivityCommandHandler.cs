using Application.Common.Enums;
using Application.Common.Models;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Rents.Commands.UpdateRentActivity;

public class UpdateRentActivityCommandHandler : IRequestHandler<UpdateRentActivityCommand, ApiResponse<int>>
{
    private readonly IRentRepository _rentRepository;

    public UpdateRentActivityCommandHandler(IRentRepository rentRepository)
    {
        _rentRepository = rentRepository;
    }

    public async Task<ApiResponse<int>> Handle(UpdateRentActivityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var rent = await _rentRepository.GetByIdAsync(request.Id);
            if (rent == null)
            {
                return new ApiResponse<int>(ETypeApiResponse.ENTITY_NOT_FOUND, request.Id, "No existe un alquiler con el Id indicado");
            }

            rent.IsActive = request.IsActive;

            await _rentRepository.UpdateAsync(rent);
            return new ApiResponse<int>(ETypeApiResponse.OK, rent.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<int>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
