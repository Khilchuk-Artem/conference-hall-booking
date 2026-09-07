using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.Reports.GetSummary;

public class GetReportSummaryQuery : IRequest<ReportSummaryDto>
{
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
}
