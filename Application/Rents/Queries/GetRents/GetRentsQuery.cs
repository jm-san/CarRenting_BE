using Application.Common.Models;
using Application.Rents.Dtos;
using Domain.Filters;
using MediatR;

namespace Application.Rents.Queries.GetRents;

public record GetRentsQuery(RentFilter Filters) : IRequest<ApiResponse<List<RentDto>>>;
