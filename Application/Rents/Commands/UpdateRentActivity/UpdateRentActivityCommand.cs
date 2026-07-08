using Application.Common.Models;
using MediatR;

namespace Application.Rents.Commands.UpdateRentActivity;

public record UpdateRentActivityCommand(string Id, bool IsActive) : IRequest<ApiResponse<string>>;
