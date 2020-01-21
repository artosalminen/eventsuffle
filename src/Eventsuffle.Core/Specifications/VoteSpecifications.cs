using Eventsuffle.Core.Entities;
using System;

namespace Eventsuffle.Core.Specifications
{
    public class VoteSpecifications : BaseSpecification<Vote>
    {
        public VoteSpecifications(Guid eventId, string personName) : base(
            i => i.EventId == eventId && i.PersonName == personName)
        {
            AddInclude(i => i.VoteSuggestedDates);
        }
    }
}
