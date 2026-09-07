using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Application.Specifications.AdditionalServices;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.AdditionalServices.GetAdditionalServices;

public class GetAdditionalServicesHandler : IRequestHandler<GetAdditionalServicesQuery, List<AdditionalServiceDto>>
{
    private readonly IRepository<AdditionalService> _additionalServiceRepository;

    public GetAdditionalServicesHandler(IRepository<AdditionalService> additionalServiceRepository)
    {
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<List<AdditionalServiceDto>> Handle(GetAdditionalServicesQuery request, CancellationToken cancellationToken)
    {
        var spec = new AdditionalServicesSpecification(request.Page, request.PageSize);
        var additionalServices = await _additionalServiceRepository.GetAll(spec);

        return additionalServices.Select(AdditionalServiceMapper.ToDto).ToList();
    }
}
