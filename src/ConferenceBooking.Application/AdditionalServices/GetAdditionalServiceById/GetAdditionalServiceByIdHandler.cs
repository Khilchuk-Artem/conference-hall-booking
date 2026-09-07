using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.AdditionalServices.GetAdditionalServiceById;

public class GetAdditionalServiceByIdHandler : IRequestHandler<GetAdditionalServiceByIdQuery, AdditionalServiceDto>
{
    private readonly IRepository<AdditionalService> _additionalServiceRepository;

    public GetAdditionalServiceByIdHandler(IRepository<AdditionalService> additionalServiceRepository)
    {
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<AdditionalServiceDto> Handle(GetAdditionalServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var additionalService = await _additionalServiceRepository.GetById(request.Id);

        if (additionalService == null) throw new NotFoundException("Additional service", request.Id);

        return AdditionalServiceMapper.ToDto(additionalService);
    }
}
