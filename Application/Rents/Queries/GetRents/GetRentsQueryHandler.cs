using Application.Common.Enums;
using Application.Common.Models;
using Application.Rents.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Rents.Queries.GetRents;

public class GetRentsQueryHandler : IRequestHandler<GetRentsQuery, ApiResponse<List<RentDto>>>
{
    private readonly IRepository<Rent, RentFilter> _rentRepository;
    private readonly IMapper _mapper;

    public GetRentsQueryHandler(IRepository<Rent, RentFilter> rentRepository, IMapper mapper)
    {
        _rentRepository = rentRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<RentDto>>> Handle(GetRentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var rents = await _rentRepository.GetAllAsync(request.Filters);
            return new ApiResponse<List<RentDto>>(ETypeApiResponse.OK, _mapper.Map<List<RentDto>>(rents));
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<RentDto>>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
