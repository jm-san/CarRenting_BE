using Application.Common.Enums;
using Application.Common.Models;
using Application.Costumers.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using FluentValidation;
using Infrastructure.Interfaces;

namespace Application.Costumers.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CustomerInDto> _validator;

    public CustomerService(
        ICustomerRepository customerRepository,
        IMapper mapper,
        IValidator<CustomerInDto> validator)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ApiResponse<List<CustomerDto>>> GetCustomers(CustomerFilter filters)
    {
        try
        {
            var customers = await _customerRepository.GetAllAsync(filters);
            return new ApiResponse<List<CustomerDto>>(ETypeApiResponse.OK, _mapper.Map<List<CustomerDto>>(customers));  // Mapear Entidad → DTO
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CustomerDto>>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    public async Task<ApiResponse<CustomerDto>> GetCustomer(string id)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return new ApiResponse<CustomerDto>(ETypeApiResponse.ENTITY_NOT_FOUND, "No existe el cliente con el Id indicado");
            }

            return new ApiResponse<CustomerDto>(ETypeApiResponse.OK, _mapper.Map<CustomerDto>(customer));  // Mapear Entidad → DTO
        }
        catch (Exception ex)
        {
            return new ApiResponse<CustomerDto>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    public async Task<ApiResponse<string>> CreateCustomer(CustomerInDto customerDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(customerDto);
            // Validar DTO
            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(ETypeApiResponse.VALIDATION_ERROR, validationResult.ToString());
            }

            var customer = _mapper.Map<Customer>(customerDto);  // Mapear DTO → Entidad
            await _customerRepository.InsertAsync(customer);

            return new ApiResponse<string>(ETypeApiResponse.OK, customer.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    public async Task<ApiResponse<string>> UpdateCustomer(string id, CustomerInDto customerDto)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return new ApiResponse<string>(ETypeApiResponse.ENTITY_NOT_FOUND, id, "No existe el cliente con el Id indicado");
            }

            customer.Name = customerDto.Name ?? customer.Name;
            customer.LastName = customerDto.LastName ?? customer.LastName;
            customer.DNI = customerDto.DNI ?? customer.DNI;
            customer.Telephone = customerDto.Telephone ?? customer.Telephone;
            customer.Birthdate = customerDto.Birthdate ?? customer.Birthdate;

            await _customerRepository.UpdateAsync(id, customer);

            return new ApiResponse<string>(ETypeApiResponse.OK, customer.Id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

    public async Task<ApiResponse<string>> DeleteCustomer(string id)
    {
        try
        {
            await _customerRepository.DeleteAsync(id);
            return new ApiResponse<string>(ETypeApiResponse.OK, id);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(ETypeApiResponse.INTERNAL_ERROR, ex.Message);
        }
    }

}
