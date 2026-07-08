using Application.Common.Models;
using MediatR;

namespace Application.Rents.Commands.DeleteRent;

public record DeleteRentCommand(string Id) : IRequest<ApiResponse<string>>;
