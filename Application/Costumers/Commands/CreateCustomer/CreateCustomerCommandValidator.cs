using FluentValidation;

namespace Application.Costumers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Customer.Name)
            .NotEmpty().WithMessage("Obligatorio indicar un nombre")
            .MaximumLength(100);

        RuleFor(x => x.Customer.LastName)
            .NotEmpty().WithMessage("Obligatorio indicar un apellido")
            .MaximumLength(100);

        RuleFor(x => x.Customer.DNI)
            .NotEmpty().WithMessage("Obligatorio indicar un DNI")
            .Matches(@"^\d{8}[A-Za-z]$").WithMessage("Formato del DNI no es válido");

        RuleFor(x => x.Customer.Telephone)
            .NotEmpty().WithMessage("Obligatorio indicar un teléfono")
            .Matches(@"^[67]\d{8}$").WithMessage("Formato de teléfono no es válido");

        RuleFor(x => x.Customer.Birthdate)
            .Must(IsValidDate).When(x => x.Customer.Birthdate.HasValue).WithMessage("Fecha de nacimiento no válida")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de nacimiento no puede ser una fecha futura")
            .LessThanOrEqualTo(DateTime.UtcNow.AddYears(-18)).WithMessage("El cliente debe de ser mayor de edad");
    }

    private bool IsValidDate(DateTime? date)
    {
        return date.HasValue && date.Value != DateTime.MinValue;
    }
}
