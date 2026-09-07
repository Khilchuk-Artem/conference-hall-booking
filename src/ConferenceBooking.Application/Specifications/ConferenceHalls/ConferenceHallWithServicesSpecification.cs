using Ardalis.Specification;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Specifications.ConferenceHalls;

public class ConferenceHallWithServicesSpecification : Specification<ConferenceHall>
{
    public ConferenceHallWithServicesSpecification()
    {
        Query.Include(x => x.AdditionalServices);
    }
}