using FluentValidation;

namespace Application.Rents.Commands.DeleteRent;

public class DeleteRentCommandValidator : AbstractValidator<DeleteRentCommand>
{
    public DeleteRentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Obligatorio indicar el Id del alquiler");
    }
}
