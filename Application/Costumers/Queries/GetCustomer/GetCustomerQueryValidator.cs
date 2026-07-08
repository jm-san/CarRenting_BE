using FluentValidation;

namespace Application.Costumers.Queries.GetCustomer;

public class GetCustomerQueryValidator : AbstractValidator<GetCustomerQuery>
{
    public GetCustomerQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Obligatorio indicar el Id del cliente");
    }
}
