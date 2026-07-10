using FluentValidation;

namespace Application.Rents.Commands.DeleteRent;

public class DeleteRentCommandValidator : AbstractValidator<DeleteRentCommand>
{
    public DeleteRentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Obligatorio indicar el Id del alquiler");
    }
}
