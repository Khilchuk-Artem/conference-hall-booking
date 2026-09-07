using Ardalis.Specification;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Specifications.Bookings;

public class ReportPeriodSpecification : Specification<Booking>
{
    public ReportPeriodSpecification(DateTimeOffset from, DateTimeOffset to)
    {
        Query.Where(booking => booking.StartTime < to && booking.EndTime > from);
    }
}
