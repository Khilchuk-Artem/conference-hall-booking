using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Application.Specifications.ConferenceHalls;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.ConferenceHalls.GetConferenceHallById;

public class GetConferenceHallByIdHandler : IRequestHandler<GetConferenceHallByIdQuery, ConferenceHallDto>
{
    private readonly IRepository<ConferenceHall> _conferenceHallRepository;

    public GetConferenceHallByIdHandler(IRepository<ConferenceHall> conferenceHallRepository)
    {
        _conferenceHallRepository = conferenceHallRepository;
    }

    public async Task<ConferenceHallDto> Handle(GetConferenceHallByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ConferenceHallWithServicesSpecification();
        var conferenceHall = await _conferenceHallRepository.GetById(request.Id, spec);

        if (conferenceHall == null) throw new NotFoundException("Conference hall", request.Id);

        return ConferenceHallMapper.ToDto(conferenceHall);
    }
}
