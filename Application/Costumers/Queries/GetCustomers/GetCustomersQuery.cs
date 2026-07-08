using Application.Common.Models;
using Application.Costumers.Dtos;
using Domain.Filters;
using MediatR;

namespace Application.Costumers.Queries.GetCustomers;

public record GetCustomersQuery(CustomerFilter Filters) : IRequest<ApiResponse<List<CustomerDto>>>;
