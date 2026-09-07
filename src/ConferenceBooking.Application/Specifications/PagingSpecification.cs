using Ardalis.Specification;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Specifications;

public class PagingSpecification : Specification<BaseEntity>
{
    public PagingSpecification(int page, int pageSize)
    {
        Query.Skip((page - 1) * pageSize);
    }
}