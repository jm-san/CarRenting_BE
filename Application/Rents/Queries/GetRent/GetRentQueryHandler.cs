using Application.Common.Enums;
using Application.Common.Models;
using Application.Rents.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Rents.Queries.GetRent;

public class GetRentQueryHandler : IRequestHandler<GetRentQuery, ApiResponse<RentDto>>
{
    private readonly IRepository<Rent, RentFilter> _rentRepository;
    private readonly IMapper _mapper;

    public GetRentQueryHandler(IRepository<Rent, RentFilter> rentRepository, IMapper mapper)
    {
        _rentRepository = rentRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<RentDto>> Handle(GetRentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var rent = await _rentRepository.GetByIdAsync(request.Id);
            return new ApiResponse<RentDto>(ETypeApiResponse.OK, _mapper.Map<RentDto>(rent));
        }
        catch (Exception ex)
        {
            return new ApiResponse<RentDto>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
