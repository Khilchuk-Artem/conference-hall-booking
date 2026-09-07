using FluentValidation;

namespace ConferenceBooking.Application.ConferenceHalls.DeleteConferenceHall;

public class DeleteConferenceHallCommandValidator : AbstractValidator<DeleteConferenceHallCommand>
{
    public DeleteConferenceHallCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}