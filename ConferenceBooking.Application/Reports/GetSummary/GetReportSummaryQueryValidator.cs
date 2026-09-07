using FluentValidation;

namespace ConferenceBooking.Application.Reports.GetSummary;

public class GetReportSummaryQueryValidator : AbstractValidator<GetReportSummaryQuery>
{
    public GetReportSummaryQueryValidator()
    {
        RuleFor(x => x.To)
            .GreaterThan(x => x.From)
            .WithMessage("Report end must be later than report start.");
    }
}
