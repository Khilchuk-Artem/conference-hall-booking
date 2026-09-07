using FluentValidation;

namespace ConferenceBooking.Application.Bookings.CreateBooking;

public sealed class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.ConferenceHallId)
            .NotEmpty()
            .WithMessage("Conference hall ID is required.");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be later than start time.");

        RuleFor(x => x)
            .Must(x => x.StartTime.Date == x.EndTime.Date)
            .WithMessage("Booking must not cross calendar day.");

        RuleFor(x => x)
            .Must(x => x.StartTime.TimeOfDay >= new TimeSpan(6, 0, 0) && x.EndTime.TimeOfDay <= new TimeSpan(23, 0, 0))
            .WithMessage("Booking must be within 06:00–23:00.");

        RuleFor(x => x.AdditionalServiceIds)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("Duplicate additional service IDs are not allowed.");

        RuleForEach(x => x.AdditionalServiceIds)
            .NotEmpty()
            .WithMessage("At least one additional service must be selected.");
    }
}