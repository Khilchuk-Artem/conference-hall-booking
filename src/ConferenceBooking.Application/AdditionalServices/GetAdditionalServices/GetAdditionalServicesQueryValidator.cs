using FluentValidation;

namespace ConferenceBooking.Application.AdditionalServices.GetAdditionalServices;

public class GetAdditionalServicesQueryValidator : AbstractValidator<GetAdditionalServicesQuery>
{
    public GetAdditionalServicesQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");
    }
}
