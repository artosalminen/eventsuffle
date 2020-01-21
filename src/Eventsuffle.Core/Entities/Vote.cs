using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Eventsuffle.Core.Entities
{
    public class Vote : BaseEntity<Guid>
    {
        public const int PERSON_NAME_MAX_LENGTH = 500;

        [Required]
        public string PersonName { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public virtual ICollection<VoteSuggestedDate> VoteSuggestedDates { get; set; }
    }
}
