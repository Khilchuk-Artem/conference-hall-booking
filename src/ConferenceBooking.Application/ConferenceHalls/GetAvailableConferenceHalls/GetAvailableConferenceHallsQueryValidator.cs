using FluentValidation;

namespace ConferenceBooking.Application.ConferenceHalls.GetAvailableConferenceHalls;

public class GetAvailableConferenceHallsQueryValidator : AbstractValidator<GetAvailableConferenceHallsQuery>
{
    public GetAvailableConferenceHallsQueryValidator()
    {
        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0.");
        
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be later than start time.");

        RuleFor(x => x)
            .Must(x => x.StartTime.Date == x.EndTime.Date)
            .WithMessage("Booking must not cross calendar day.");

        RuleFor(x => x)
            .Must(x => x.StartTime.TimeOfDay >= new TimeSpan(6, 0, 0) && x.EndTime.TimeOfDay <= new TimeSpan(23, 0, 0))
            .WithMessage("Booking must be within 06:00–23:00.");
        
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");
        
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");
    }
}
