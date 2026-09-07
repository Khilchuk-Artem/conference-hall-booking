using Ardalis.Specification.EntityFrameworkCore;
using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.Reports.Models;
using ConferenceBooking.Application.Specifications.Bookings;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Data.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly ConferenceBookingDbContext _context;

    public ReportRepository(ConferenceBookingDbContext context)
    {
        _context = context;
    }

    public async Task<ReportSummary> GetSummary(DateTimeOffset from, DateTimeOffset to)
    {
        var bookings = _context.Bookings
            .AsNoTracking()
            .WithSpecification(new ReportPeriodSpecification(from, to));

        // hours are clipped to the report window while revenue keeps the booking total
        var bookingsWithDuration = bookings.Select(booking => new
        {
            booking.ConferenceHallId,
            booking.HallName,
            booking.TotalCost,
            booking.StartTime,
            booking.EndTime,
            DurationHours = ((booking.EndTime > to ? to : booking.EndTime) -
                             (booking.StartTime < from ? from : booking.StartTime)).TotalHours
        });

        var totals = await bookingsWithDuration
            .GroupBy(_ => 1)
            .Select(group => new
            {
                BookingsCount = group.Count(),
                TotalHours = group.Sum(booking => booking.DurationHours),
                TotalRevenue = group.Sum(booking => booking.TotalCost)
            })
            .SingleOrDefaultAsync();

        var halls = await _context.ConferenceHalls
            .AsNoTracking()
            .Select(hall => new HallReport
            {
                ConferenceHallId = hall.Id,
                HallName = hall.Name,
                BookingsCount = bookingsWithDuration.Count(booking => booking.ConferenceHallId == hall.Id),
                TotalHours = bookingsWithDuration
                    .Where(booking => booking.ConferenceHallId == hall.Id)
                    .Select(booking => (double?)booking.DurationHours)
                    .Sum() ?? 0,
                Revenue = bookingsWithDuration
                    .Where(booking => booking.ConferenceHallId == hall.Id)
                    .Select(booking => (decimal?)booking.TotalCost)
                    .Sum() ?? 0
            })
            .OrderByDescending(hall => hall.BookingsCount)
            .ThenBy(hall => hall.HallName)
            .ToListAsync();

        var services = await bookings
            .SelectMany(booking => booking.AdditionalServices.Select(service => new
            {
                service.AdditionalServiceId,
                service.ServiceName,
                service.Price
            }))
            .GroupBy(service => new { service.AdditionalServiceId, service.ServiceName })
            .Select(group => new AdditionalServiceReport
            {
                AdditionalServiceId = group.Key.AdditionalServiceId,
                ServiceName = group.Key.ServiceName,
                UsageCount = group.Count(),
                Revenue = group.Sum(service => service.Price)
            })
            .OrderByDescending(service => service.UsageCount)
            .ThenByDescending(service => service.Revenue)
            .ThenBy(service => service.ServiceName)
            .ToListAsync();

        return new ReportSummary
        {
            From = from,
            To = to,
            BookingsCount = totals?.BookingsCount ?? 0,
            TotalHours = totals?.TotalHours ?? 0,
            TotalRevenue = totals?.TotalRevenue ?? 0,
            Halls = halls,
            MostPopularAdditionalServices = services
        };
    }
}
