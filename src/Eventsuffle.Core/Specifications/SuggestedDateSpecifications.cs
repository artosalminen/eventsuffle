using Eventsuffle.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventsuffle.Core.Specifications
{
    public class SuggestedDateSpecifications: BaseSpecification<SuggestedDate>
    {
        public SuggestedDateSpecifications(Guid eventId, IReadOnlyList<DateTime> dates) : base(
            i => i.EventId == eventId && 
            dates.Select(date => date.Date).ToList().Contains(i.Date.Date))
        {
            AddInclude(i => i.VoteSuggestedDates);
        }
    }
}
