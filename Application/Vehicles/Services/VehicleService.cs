using Application.Common.Enums;
using Application.Common.Models;
using Application.Vehicles.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using FluentValidation;
using Infrastructure.Interfaces;

namespace Application.Vehicles.Services;

public class VehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<VehicleInDto> _validator;

    public VehicleService(
        IVehicleRepository vehicleRepository,
        IMapper mapper,
        IValidator<VehicleInDto> validator)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ApiResponse<List<VehicleDto>>> GetVehicles(VehicleFilter filters)
    {
        try
        {
            var vehicles = await _vehicleRepository.GetAllAsync(filters);
            return new ApiResponse<List<VehicleDto>>(ETypeApiResponse.OK, _mapper.Map<List<VehicleDto>>(vehicles));  // Mapear Entidad → DTO
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<VehicleDto>>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    public async Task<ApiResponse<VehicleDto>> GetVehicle(string id)
    {
        try
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            return new ApiResponse<VehicleDto>(ETypeApiResponse.OK, _mapper.Map<VehicleDto>(vehicle));  // Mapear Entidad → DTO
        }
        catch (Exception ex)
        {
            return new ApiResponse<VehicleDto>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }

    }

    public async Task<ApiResponse<string>> CreateVehicle(VehicleInDto vehicleDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(vehicleDto);
            // Validar DTO
            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(ETypeApiResponse.VALIDATION_ERROR, validationResult.ToString());
            }

            var vehicle = _mapper.Map<Vehicle>(vehicleDto);  // Mapear DTO → Entidad

            await _vehicleRepository.InsertAsync(vehicle);
            return new ApiResponse<string>(ETypeApiResponse.OK, vehicle.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    public async Task<ApiResponse<string>> UpdateVehicle(string id, VehicleInDto vehicleDto)
    {
        try
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                return new ApiResponse<string>(ETypeApiResponse.ENTITY_NOT_FOUND, id, "No existe el vehículo con el Id indicado");
            }


            vehicle.Brand = vehicleDto.Brand ?? vehicle.Brand;
            vehicle.Model = vehicleDto.Model ?? vehicle.Model;
            vehicle.NumberPlate = vehicleDto.NumberPlate ?? vehicle.NumberPlate;
            vehicle.ManufacturingDate = vehicleDto.ManufacturingDate ?? vehicle.ManufacturingDate;

            await _vehicleRepository.UpdateAsync(id, vehicle);

            return new ApiResponse<string>(ETypeApiResponse.OK, vehicle.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);

        }
    }

    public async Task<ApiResponse<string>> DeleteVehicle(string id)
    {
        try
        {
            await _vehicleRepository.DeleteAsync(id);
            return new ApiResponse<string>(ETypeApiResponse.OK, id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

}