using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.AdditionalServices.EditAdditionalService;

public class EditAdditionalServiceHandler : IRequestHandler<EditAdditionalServiceCommand, AdditionalServiceDto>
{
    private readonly IRepository<AdditionalService> _additionalServiceRepository;

    public EditAdditionalServiceHandler(IRepository<AdditionalService> additionalServiceRepository)
    {
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<AdditionalServiceDto> Handle(EditAdditionalServiceCommand request, CancellationToken cancellationToken)
    {
        var additionalService = await _additionalServiceRepository.GetById(request.Id);

        if (additionalService == null) throw new NotFoundException("Additional service", request.Id);

        additionalService.Name = request.Name;
        additionalService.Price = request.Price;

        await _additionalServiceRepository.Update(additionalService);

        return AdditionalServiceMapper.ToDto(additionalService);
    }
}
