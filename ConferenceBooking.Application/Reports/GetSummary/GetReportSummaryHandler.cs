using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Mapping;
using MediatR;

namespace ConferenceBooking.Application.Reports.GetSummary;

public class GetReportSummaryHandler : IRequestHandler<GetReportSummaryQuery, ReportSummaryDto>
{
    private readonly IReportRepository _reportRepository;

    public GetReportSummaryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<ReportSummaryDto> Handle(GetReportSummaryQuery request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetSummary(request.From.ToUniversalTime(), request.To.ToUniversalTime());
        return ReportMapper.ToDto(report);
    }
}
