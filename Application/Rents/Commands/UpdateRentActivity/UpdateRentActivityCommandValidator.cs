using FluentValidation;

namespace Application.Rents.Commands.UpdateRentActivity;

public class UpdateRentActivityCommandValidator : AbstractValidator<UpdateRentActivityCommand>
{
    public UpdateRentActivityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Obligatorio indicar el Id del alquiler");
    }
}
