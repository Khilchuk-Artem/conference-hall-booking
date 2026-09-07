using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.AdditionalServices.DeleteAdditionalService;

public class DeleteAdditionalServiceHandler : IRequestHandler<DeleteAdditionalServiceCommand, AdditionalServiceDto>
{
    private readonly IRepository<AdditionalService> _additionalServiceRepository;

    public DeleteAdditionalServiceHandler(IRepository<AdditionalService> additionalServiceRepository)
    {
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<AdditionalServiceDto> Handle(DeleteAdditionalServiceCommand request, CancellationToken cancellationToken)
    {
        var additionalService = await _additionalServiceRepository.GetById(request.Id);

        if (additionalService == null) throw new NotFoundException("Additional service", request.Id);

        var deletedService = await _additionalServiceRepository.Delete(request.Id);

        if (deletedService == null) throw new NotFoundException("Additional service", request.Id);

        return AdditionalServiceMapper.ToDto(deletedService);
    }
}
