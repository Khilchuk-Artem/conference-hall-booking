using FluentValidation;

namespace ConferenceBooking.Application.ConferenceHalls.EditConferenceHall;

public class EditConferenceHallCommandValidator : AbstractValidator<EditConferenceHallCommand>
{
    public EditConferenceHallCommandValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Name must be between 1 and 200 characters long.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0.");

        RuleFor(x => x.RentRate)
            .GreaterThan(0)
            .WithMessage("Rent rate must be greater than 0.");

        RuleFor(x => x.AdditionalServiceIds)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("Duplicate additional service IDs are not allowed.");

        RuleForEach(x => x.AdditionalServiceIds)
            .NotEmpty()
            .WithMessage("At least one additional service must be selected.");
        
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}