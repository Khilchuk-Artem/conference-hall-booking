using Asp.Versioning;
using ConferenceBooking.Application.Bookings.CreateBooking;
using ConferenceBooking.Application.Bookings.GetBookings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BookingsController : Controller
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateBooking")]
    public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
    {
        var id = await _mediator.Send(command);
        if (id == Guid.Empty) return BadRequest("Booking was not created.");

        return CreatedAtAction(nameof(Create), new { version = "1", id }, id);
    }

    [HttpGet(Name = "GetBookings")]
    public async Task<IActionResult> Get([FromQuery] GetBookingsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
