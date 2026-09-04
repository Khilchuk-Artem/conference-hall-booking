using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.Specifications.ConferenceHalls;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Pricing;
using MediatR;

namespace ConferenceBooking.Application.Bookings.CreateBooking;

public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IRepository<Booking> _bookingRepository;
    private readonly IRepository<ConferenceHall> _conferenceHallRepository;
    private readonly RentPriceCalculator _rentPriceCalculator;
    
    public CreateBookingHandler(IRepository<Booking> bookingRepository, IRepository<ConferenceHall> conferenceHallRepository, RentPriceCalculator rentPriceCalculator)
    {
        _bookingRepository = bookingRepository;
        _conferenceHallRepository = conferenceHallRepository;
        _rentPriceCalculator = rentPriceCalculator;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var servicesSpecification = new ConferenceHallWithServicesSpecification();
        var conferenceHall = await _conferenceHallRepository.GetById(request.ConferenceHallId, servicesSpecification);
        
        var hallCost =
            _rentPriceCalculator.CalculateTotalPrice(conferenceHall.RentRate, request.StartTime, request.EndTime) ??
            throw new Exception(
                "Invalid rent price calculation"
            );
        
        var services = conferenceHall
            .AdditionalServices
            .Where(s=>request.AdditionalServiceIds.Contains(s.Id))
            .ToList();
        var servicesCost = services.Sum(s => s.Price);
        
        var booking = new Booking()
        {
            ConferenceHallId = conferenceHall.Id,
            
            HallName = conferenceHall.Name,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            HallHourlyRate = conferenceHall.RentRate,
            
            TotalServicesCost = servicesCost,
            HallCost = hallCost,
            TotalCost = hallCost + servicesCost,
            
            AdditionalServices = services.Select(s=>
                new BookingService()
                {
                    ServiceName = s.Name,
                    AdditionalServiceId = s.Id,
                    Price = s.Price
                }).ToList()
        };
        
        throw new NotImplementedException();
    }
}