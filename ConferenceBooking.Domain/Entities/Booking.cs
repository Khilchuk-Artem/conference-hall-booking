using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceBooking.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public Guid ConferenceHallId { get; set; }

        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }

        public string HallName {  get; set; }
        public decimal HallHourlyRate { get; set; }
        
        public decimal HallCost {  get; set; }
        public decimal TotalServicesCost { get; set; }
        public decimal TotalCost {  get; set; }

        public List<BookingService> AdditionalServices { get; set; }
        public ConferenceHall ConferenceHall { get; set; }
    }
}
