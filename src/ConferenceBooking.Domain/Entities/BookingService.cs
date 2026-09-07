using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceBooking.Domain.Entities
{
    public class BookingService : BaseEntity
    {
        public Guid BookingId { get; set; }
        public Guid AdditionalServiceId { get; set; }
        public string ServiceName {  get; set; }
        public decimal Price {  get; set; }

        public AdditionalService AdditionalService { get; set; }
        public Booking Booking { get; set; }
    }
}
