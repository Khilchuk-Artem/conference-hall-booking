using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Application.Specifications.ConferenceHalls;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.ConferenceHalls.GetAvailableConferenceHalls;

public class GetAvailableConferenceHallHandler : IRequestHandler<GetAvailableConferenceHallsQuery, List<ConferenceHallDto>>
{
    private readonly IRepository<ConferenceHall> _conferenceHallRepository;

    public GetAvailableConferenceHallHandler(IRepository<ConferenceHall> conferenceHallRepository)
    {
        _conferenceHallRepository = conferenceHallRepository;
    }

    public async Task<List<ConferenceHallDto>> Handle(GetAvailableConferenceHallsQuery request, CancellationToken cancellationToken)
    {
        var availableHallsSpec = new AvailableConferenceHallsSpecification(request.StartTime, request.EndTime, request.Capacity, request.Page, request.PageSize);
        
        var availableHalls = await _conferenceHallRepository.GetAll(availableHallsSpec);

        var res = availableHalls.Select(h => ConferenceHallMapper.ToDto(h)).ToList();
        
        return res;
    }
}