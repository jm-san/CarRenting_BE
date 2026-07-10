using Application.Common.Enums;
using Application.Common.Models;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Rents.Commands.DeleteRent;

public class DeleteRentCommandHandler : IRequestHandler<DeleteRentCommand, ApiResponse<int>>
{
    private readonly IRentRepository _rentRepository;

    public DeleteRentCommandHandler(IRentRepository rentRepository)
    {
        _rentRepository = rentRepository;
    }

    public async Task<ApiResponse<int>> Handle(DeleteRentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _rentRepository.DeleteAsync(request.Id);
            return new ApiResponse<int>(ETypeApiResponse.OK, request.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<int>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
