using Application.Common.Enums;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Rents.Commands.CreateRent;

public class CreateRentCommandHandler : IRequestHandler<CreateRentCommand, ApiResponse<int>>
{
    private readonly IRentRepository _rentRepository;
    private readonly IMapper _mapper;

    public CreateRentCommandHandler(IRentRepository rentRepository, IMapper mapper)
    {
        _rentRepository = rentRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<int>> Handle(CreateRentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var rent = _mapper.Map<Rent>(request.Rent);
            rent.IsActive = true;

            //Búsqueda de si el cliente tiene alquileres activos
            var activeRents = await _rentRepository.GetAllAsync(new RentFilter
            {
                CustomerId = rent.CustomerId,
                IsActive = true
            });

            if (activeRents.Count > 0)
            {
                return new ApiResponse<int>(ETypeApiResponse.CUSTOMER_WITH_ACTIVE_RENT, "El cliente ya tiene un alquiler activo");
            }

            await _rentRepository.InsertAsync(rent);
            return new ApiResponse<int>(ETypeApiResponse.OK, rent.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<int>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}
