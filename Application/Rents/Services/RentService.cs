using Application.Common.Enums;
using Application.Common.Models;
using Application.Rents.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using FluentValidation;
using Infrastructure.Interfaces;

namespace Application.Rents.Services;

public class RentService
{
    private readonly IRentRepository _rentRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<RentInDto> _validator;

    public RentService(
        IRentRepository rentRepository,
        IMapper mapper,
        IValidator<RentInDto> validator)
    {
        _rentRepository = rentRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ApiResponse<List<RentDto>>> GetRents(RentFilter filters)
    {
        try
        {
            var rents = await _rentRepository.GetAllAsync(filters);
            return new ApiResponse<List<RentDto>>(ETypeApiResponse.OK, _mapper.Map<List<RentDto>>(rents));  // Mapear Entidad → DTO
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<RentDto>>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    public async Task<ApiResponse<RentDto>> GetRent(string id)
    {
        try
        {
            var rent = await _rentRepository.GetByIdAsync(id);
            return new ApiResponse<RentDto>(ETypeApiResponse.OK, _mapper.Map<RentDto>(rent));  // Mapear Entidad → DTO
        }
        catch (Exception ex)
        {
            return new ApiResponse<RentDto>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    public async Task<ApiResponse<string>> CreateRent(RentInDto rentDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(rentDto);
            // Validar DTO
            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(ETypeApiResponse.VALIDATION_ERROR, validationResult.ToString());
            }

            var rent = _mapper.Map<Rent>(rentDto);  // Mapear DTO → Entidad

            //Búsqueda de si el cliente tiene alquileres activos
            var rents = await _rentRepository.GetAllAsync(new RentFilter
            {
                CustomerId = rent.CustomerId,
                IsActive = true
            });

            if (rents.Count > 0)
            {
                return new ApiResponse<string>(ETypeApiResponse.CUSTOMER_WITH_ACTIVE_RENT, "El cliente ya tiene un alquiler activo");
            }

            await _rentRepository.InsertAsync(rent);
            return new ApiResponse<string>(ETypeApiResponse.OK, rent.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    public async Task<ApiResponse<string>> UpdateRentActivity(string id, RentInDto rentDto)
    {
        try
        {
            var rent = await _rentRepository.GetByIdAsync(id);
            if (rent == null)
            {
                return new ApiResponse<string>(ETypeApiResponse.ENTITY_NOT_FOUND, id, "No existe un alquiler con el Id indicado");

            }

            rent.IsActive = rentDto.IsActive;

            await _rentRepository.UpdateAsync(id, rent);
            return new ApiResponse<string>(ETypeApiResponse.OK, rent.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    public async Task<ApiResponse<string>> DeleteRent(string id)
    {
        try
        {
            await _rentRepository.DeleteAsync(id);
            return new ApiResponse<string>(ETypeApiResponse.OK, id);

        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }
}