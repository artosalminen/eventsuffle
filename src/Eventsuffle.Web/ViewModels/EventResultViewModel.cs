using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eventsuffle.Web.ViewModels
{
    public class EventResultViewModel: BaseViewModel
    {
        /// <summary>
        /// Name of the event.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Dates that are suitable for all participants.
        /// </summary>
        public IEnumerable<DateVotesViewModel> SuitableDates { get; set; }
    }
}
