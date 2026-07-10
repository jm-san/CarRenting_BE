using Application.Common.Models;
using MediatR;

namespace Application.Rents.Commands.UpdateRentActivity;

public record UpdateRentActivityCommand(int Id, bool IsActive) : IRequest<ApiResponse<int>>;
