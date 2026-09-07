using FluentValidation;

namespace ConferenceBooking.Application.AdditionalServices.DeleteAdditionalService;

public class DeleteAdditionalServiceCommandValidator : AbstractValidator<DeleteAdditionalServiceCommand>
{
    public DeleteAdditionalServiceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}
