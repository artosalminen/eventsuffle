using System;

namespace Eventsuffle.Core.Entities
{
    public class VoteSuggestedDate: BaseEntity<Guid>
    {
        public Guid VoteId { get; set; }
        public Vote Vote { get; set; }
        public Guid SuggestedDateId { get; set; }
        public SuggestedDate SuggestedDate { get; set; }
    }
}
