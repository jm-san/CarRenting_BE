using Application.Common.Models;
using Application.Costumers.Dtos;
using MediatR;

namespace Application.Costumers.Commands.CreateCustomer;

public record CreateCustomerCommand(CreateCustomerDto Customer) : IRequest<ApiResponse<string>>;
