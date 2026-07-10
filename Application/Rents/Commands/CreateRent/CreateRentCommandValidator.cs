using FluentValidation;

namespace Application.Rents.Commands.CreateRent;

public class CreateRentCommandValidator : AbstractValidator<CreateRentCommand>
{
    public CreateRentCommandValidator()
    {
        RuleFor(x => x.Rent.CustomerId)
            .GreaterThan(0).WithMessage("Obligatorio indicar un cliente");

        RuleFor(x => x.Rent.VehicleId)
            .GreaterThan(0).WithMessage("Obligatorio indicar un vehículo");

        RuleFor(x => x.Rent.RentStartDate)
            .NotNull().WithMessage("Fecha de inicio del alquiler no válida")
            .LessThanOrEqualTo(x => x.Rent.RentEndDate).WithMessage("La fecha de inicio del alquiler no puede ser posterior a la fecha de fin");

        RuleFor(x => x.Rent.RentEndDate)
            .NotNull().WithMessage("Fecha de fin del alquiler no válida");

        RuleFor(x => x.Rent.TotalPrice)
            .NotEmpty().WithMessage("Obligatorio indicar un precio del alquiler");
    }
}
