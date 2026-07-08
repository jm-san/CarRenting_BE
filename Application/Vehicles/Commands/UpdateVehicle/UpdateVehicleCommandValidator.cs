using FluentValidation;

namespace Application.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
    public UpdateVehicleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Obligatorio indicar el Id del vehículo");

        RuleFor(x => x.Vehicle.Brand)
            .MaximumLength(100)
            .When(x => x.Vehicle.Brand is not null);

        RuleFor(x => x.Vehicle.Model)
            .MaximumLength(100)
            .When(x => x.Vehicle.Model is not null);

        RuleFor(x => x.Vehicle.NumberPlate)
            .Matches(@"^\d{4}[A-Za-z]{3}$").WithMessage("Formato de la matrícula no es válido")
            .When(x => x.Vehicle.NumberPlate is not null);

        RuleFor(x => x.Vehicle.ManufacturingDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de fabricación no puede ser en el futuro.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.AddYears(-5)).WithMessage("El vehículo no puede tener más de 5 años de antigüedad.")
            .When(x => x.Vehicle.ManufacturingDate.HasValue);
    }
}
