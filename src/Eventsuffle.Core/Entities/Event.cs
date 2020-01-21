using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eventsuffle.Core.Entities
{
    public class Event: BaseEntity<Guid>
    {
        public const int NAME_MAX_LENGTH = 500;

        [Required]
        public string Name { get; set; }
        public ICollection<SuggestedDate> SuggestedDates { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
