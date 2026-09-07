using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Reports.Models;
using Riok.Mapperly.Abstractions;

namespace ConferenceBooking.Application.Mapping;

[Mapper]
public partial class ReportMapper
{
    public static partial ReportSummaryDto ToDto(ReportSummary source);
    public static partial HallReportDto ToDto(HallReport source);
    public static partial AdditionalServiceReportDto ToDto(AdditionalServiceReport source);
}
