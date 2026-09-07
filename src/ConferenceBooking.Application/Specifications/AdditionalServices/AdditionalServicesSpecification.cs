using Ardalis.Specification;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Specifications.AdditionalServices;

public class AdditionalServicesSpecification : Specification<AdditionalService>
{
    public AdditionalServicesSpecification(int page, int pageSize)
    {
        Query.OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }
}
