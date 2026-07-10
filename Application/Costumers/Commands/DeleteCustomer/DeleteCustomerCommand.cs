using Application.Common.Models;
using MediatR;

namespace Application.Costumers.Commands.DeleteCustomer;

public record DeleteCustomerCommand(int Id) : IRequest<ApiResponse<int>>;
