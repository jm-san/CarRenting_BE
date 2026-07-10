using Application.Common.Models;
using Application.Rents.Dtos;
using MediatR;

namespace Application.Rents.Commands.CreateRent;

public record CreateRentCommand(CreateRentDto Rent) : IRequest<ApiResponse<int>>;
