using Ardalis.Specification;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Specifications.Bookings;

public class OverlappingBookingsSpecification : Specification<Booking>
{
    public OverlappingBookingsSpecification(Guid conferenceHallId, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        Query
            .Where(b => b.ConferenceHallId == conferenceHallId &&
                        b.StartTime < endTime &&
                        b.EndTime > startTime)
            .Take(1);

    }
}