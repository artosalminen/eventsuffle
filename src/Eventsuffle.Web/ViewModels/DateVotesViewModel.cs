using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eventsuffle.Web.ViewModels
{
    public class DateVotesViewModel
    {
        /// <summary>
        /// Date when the votes are valid.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Names of people who voted the date.
        /// </summary>
        public IEnumerable<string> People { get; set; }
    }
}
