using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eventsuffle.Web.ViewModels
{
    public class EventViewModel: BaseViewModel
    {
        /// <summary>
        /// Name of the event.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Dates when the event is suggested to take place.
        /// </summary>
        [Required]
        public IEnumerable<DateTime> Dates { get; set; }

        /// <summary>
        /// Votes for the event.
        /// </summary>
        public IEnumerable<DateVotesViewModel> Votes { get; set; }
    }
}
