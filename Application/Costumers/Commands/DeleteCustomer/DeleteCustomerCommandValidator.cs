using FluentValidation;

namespace Application.Costumers.Commands.DeleteCustomer;

public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Obligatorio indicar el Id del cliente");
    }
}
