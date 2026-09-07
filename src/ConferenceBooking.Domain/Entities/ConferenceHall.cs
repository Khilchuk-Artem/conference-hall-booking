using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceBooking.Domain.Entities
{
    public class ConferenceHall : BaseEntity
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal RentRate { get; set; }

        public List<AdditionalService> AdditionalServices { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
