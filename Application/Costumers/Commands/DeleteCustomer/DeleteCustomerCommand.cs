using Application.Common.Models;
using MediatR;

namespace Application.Costumers.Commands.DeleteCustomer;

public record DeleteCustomerCommand(string Id) : IRequest<ApiResponse<string>>;
