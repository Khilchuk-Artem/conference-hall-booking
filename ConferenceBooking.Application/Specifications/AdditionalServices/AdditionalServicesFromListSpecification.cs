using Ardalis.Specification;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Specifications.AdditionalServices;

public class AdditionalServicesFromListSpecification : Specification<AdditionalService>
{
    public AdditionalServicesFromListSpecification(List<Guid> ids)
    {
        Query.Where(x => ids.Contains(x.Id));
    }
}