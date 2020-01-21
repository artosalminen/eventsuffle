using Eventsuffle.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eventsuffle.Web.ViewModels
{
    public class EventCreateViewModel
    {
        /// <summary>
        /// Name of the event.
        /// </summary>
        [Required]
        [MaxLength(Event.NAME_MAX_LENGTH)]
        [MinLength(1)]
        public string Name { get; set; }
        
        /// <summary>
        /// Available dates for participants to choose from.
        /// </summary>
        [Required]
        [MinLength(1)]
        public IEnumerable<DateTime> Dates { get; set; }
    }
}
