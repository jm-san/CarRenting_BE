using Application.Common.Models;
using Application.Costumers.Dtos;
using MediatR;

namespace Application.Costumers.Queries.GetCustomer;

public record GetCustomerQuery(int Id) : IRequest<ApiResponse<CustomerDto>>;
