using FluentValidation;

namespace Application.Costumers.Queries.GetCustomer;

public class GetCustomerQueryValidator : AbstractValidator<GetCustomerQuery>
{
    public GetCustomerQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Obligatorio indicar el Id del cliente");
    }
}
