using FluentValidation;

namespace Application.Rents.Commands.UpdateRentActivity;

public class UpdateRentActivityCommandValidator : AbstractValidator<UpdateRentActivityCommand>
{
    public UpdateRentActivityCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Obligatorio indicar el Id del alquiler");
    }
}
