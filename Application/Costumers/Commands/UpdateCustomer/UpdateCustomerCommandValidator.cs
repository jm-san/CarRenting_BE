using FluentValidation;

namespace Application.Costumers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Obligatorio indicar el Id del cliente");

        RuleFor(x => x.Customer.Name)
            .MaximumLength(100)
            .When(x => x.Customer.Name is not null);

        RuleFor(x => x.Customer.LastName)
            .MaximumLength(100)
            .When(x => x.Customer.LastName is not null);

        RuleFor(x => x.Customer.DNI)
            .Matches(@"^\d{8}[A-Za-z]$").WithMessage("Formato del DNI no es válido")
            .When(x => x.Customer.DNI is not null);

        RuleFor(x => x.Customer.Telephone)
            .Matches(@"^[67]\d{8}$").WithMessage("Formato de teléfono no es válido")
            .When(x => x.Customer.Telephone is not null);

        RuleFor(x => x.Customer.Birthdate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de nacimiento no puede ser una fecha futura")
            .LessThanOrEqualTo(DateTime.UtcNow.AddYears(-18)).WithMessage("El cliente debe de ser mayor de edad")
            .When(x => x.Customer.Birthdate.HasValue);
    }
}
