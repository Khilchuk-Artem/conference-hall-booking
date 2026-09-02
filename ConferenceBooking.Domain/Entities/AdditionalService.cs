using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceBooking.Domain.Entities
{
    public class AdditionalService : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
