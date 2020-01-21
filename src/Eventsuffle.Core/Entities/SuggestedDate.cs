using System;
using System.Collections.Generic;

namespace Eventsuffle.Core.Entities
{
    public class SuggestedDate: BaseEntity<Guid>
    {
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<VoteSuggestedDate> VoteSuggestedDates { get; set; }
    }
}
