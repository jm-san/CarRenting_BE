using Application.Common.Models;
using Application.Rents.Dtos;
using MediatR;

namespace Application.Rents.Queries.GetRent;

public record GetRentQuery(string Id) : IRequest<ApiResponse<RentDto>>;
