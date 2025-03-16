using Application.Rents.Dtos;
using FluentValidation;

namespace Application.Rents.Validators;

public class RentInDtoValidator : AbstractValidator<RentInDto>
{
    public RentInDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Obligatorio indicar un cliente");

        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("Obligatorio indicar un vehículo");

        RuleFor(x => x.RentStartDate)
            .Must(IsValidDate).When(bd => bd.RentStartDate.HasValue).WithMessage("Fecha de inicio del alquiler no válida")
            .LessThanOrEqualTo(x => x.RentEndDate).WithMessage("La fecha de inicio del alquiler no puede ser  posterior a la fecha de fin");

        RuleFor(x => x.RentEndDate)
            .Must(IsValidDate).When(bd => bd.RentStartDate.HasValue).WithMessage("Fecha de fin del alquiler no válida");

        RuleFor(x => x.TotalPrice)
            .NotEmpty().WithMessage("Obligatorio indicar un precio del alquiler");

        RuleFor(x => x.IsActive)
            .NotEmpty().WithMessage("Obligatorio indicar si el alquiler está activo");

    }

    private bool IsValidDate(DateTime? date)
    {
        return date.HasValue && date.Value != DateTime.MinValue;
    }
}
