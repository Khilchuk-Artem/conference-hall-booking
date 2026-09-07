using FluentValidation;

namespace ConferenceBooking.Application.AdditionalServices.CreateAdditionalService;

public sealed class CreateAdditionalServiceCommandValidator : AbstractValidator<CreateAdditionalServiceCommand>
{
    public CreateAdditionalServiceCommandValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Name must be between 1 and 200 characters long.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }
}
