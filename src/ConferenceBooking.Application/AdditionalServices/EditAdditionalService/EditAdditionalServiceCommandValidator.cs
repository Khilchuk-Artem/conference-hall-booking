using FluentValidation;

namespace ConferenceBooking.Application.AdditionalServices.EditAdditionalService;

public sealed class EditAdditionalServiceCommandValidator : AbstractValidator<EditAdditionalServiceCommand>
{
    public EditAdditionalServiceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

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
