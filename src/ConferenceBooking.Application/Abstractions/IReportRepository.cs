using ConferenceBooking.Application.Reports.Models;

namespace ConferenceBooking.Application.Abstractions;

public interface IReportRepository
{
    public Task<ReportSummary> GetSummary(DateTimeOffset from, DateTimeOffset to);
}
