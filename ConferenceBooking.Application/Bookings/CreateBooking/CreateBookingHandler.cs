using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Application.Specifications.Bookings;
using ConferenceBooking.Application.Specifications.ConferenceHalls;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Pricing;
using MediatR;

using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Mapping;

namespace ConferenceBooking.Application.Bookings.CreateBooking;

public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, BookingDto>
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

    public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var overlappingBookingsSpecification = new OverlappingBookingsSpecification(request.ConferenceHallId, request.StartTime, request.EndTime);
        var bookings = await _bookingRepository.GetAll(overlappingBookingsSpecification);
        
        if (bookings.Any()) throw new ConflictException("The conference hall is already booked for the selected time.");
        
        var servicesSpecification = new ConferenceHallWithServicesSpecification();
        var conferenceHall = await _conferenceHallRepository.GetById(request.ConferenceHallId, servicesSpecification);
        if (conferenceHall == null) throw new NotFoundException("Conference hall", request.ConferenceHallId);
        
        var hallCost =
            _rentPriceCalculator.CalculateTotalPrice(conferenceHall.RentRate, request.StartTime, request.EndTime) ??
            throw new InvalidOperationException("Rent price could not be calculated.");
        
        var services = conferenceHall
            .AdditionalServices
            .Where(s=>request.AdditionalServiceIds.Contains(s.Id))
            .ToList();
        
        if (services.Count != request.AdditionalServiceIds.Distinct().Count()) throw new BadRequestException("One or more additional services were not found.");
        
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
        
        await _bookingRepository.Add(booking);
        
        return BookingMapper.ToDto(booking);
    }
}
