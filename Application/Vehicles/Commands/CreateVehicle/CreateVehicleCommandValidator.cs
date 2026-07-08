using FluentValidation;

namespace Application.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.Vehicle.Brand)
            .NotEmpty().WithMessage("Obligatorio indicar una marca")
            .MaximumLength(100);

        RuleFor(x => x.Vehicle.Model)
            .NotEmpty().WithMessage("Obligatorio indicar un modelo")
            .MaximumLength(100);

        RuleFor(x => x.Vehicle.NumberPlate)
            .NotEmpty().WithMessage("Obligatorio indicar una matrícula")
            .Matches(@"^\d{4}[A-Za-z]{3}$").WithMessage("Formato de la matrícula no es válido");

        RuleFor(x => x.Vehicle.ManufacturingDate)
            .NotNull().WithMessage("La fecha de fabricación es obligatoria.")
            .Must(IsValidDate).WithMessage("La fecha de fabricación no es válida.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de fabricación no puede ser en el futuro.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.AddYears(-5)).WithMessage("El vehículo no puede tener más de 5 años de antigüedad.");
    }

    private bool IsValidDate(DateTime? date)
    {
        return date.HasValue && date.Value != DateTime.MinValue;
    }
}
