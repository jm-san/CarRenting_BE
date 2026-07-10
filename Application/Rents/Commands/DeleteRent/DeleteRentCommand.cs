using Application.Common.Models;
using MediatR;

namespace Application.Rents.Commands.DeleteRent;

public record DeleteRentCommand(int Id) : IRequest<ApiResponse<int>>;
