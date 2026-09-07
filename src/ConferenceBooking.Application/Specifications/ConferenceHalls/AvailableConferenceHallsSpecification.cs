using Ardalis.Specification;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Specifications.ConferenceHalls;

public class AvailableConferenceHallsSpecification : Specification<ConferenceHall>
{
    public AvailableConferenceHallsSpecification(DateTimeOffset startTime, DateTimeOffset endTime, int capacity, int page, int pageSize)
    {
        // adjacent intervals are allowed because their boundaries do not overlap
        Query.Where(ch => ch.Capacity >= capacity &&
                          ch.Bookings.All(b =>
                              b.EndTime <= startTime ||
                              b.StartTime >= endTime))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(x => x.AdditionalServices);
    }
}
