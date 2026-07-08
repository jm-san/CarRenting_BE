using Application.Common.Models;
using Application.Costumers.Dtos;
using MediatR;

namespace Application.Costumers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(string Id, UpdateCustomerDto Customer) : IRequest<ApiResponse<string>>;
