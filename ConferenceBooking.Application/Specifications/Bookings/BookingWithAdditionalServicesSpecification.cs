using Ardalis.Specification;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Specifications.Bookings;

public class BookingWithAdditionalServicesSpecification : Specification<Booking>
{
    public BookingWithAdditionalServicesSpecification(int? page = null, int? pageSize = null)
    {
        Query.Include(x => x.AdditionalServices)
            .OrderBy(x => x.StartTime);
        
        if (page.HasValue && pageSize.HasValue)
        {
            Query.Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }
    }
}