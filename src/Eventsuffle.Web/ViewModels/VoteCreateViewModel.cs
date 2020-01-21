using Eventsuffle.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eventsuffle.Web.ViewModels
{
    public class VoteCreateViewModel
    {
        /// <summary>
        /// Name of the person adding the vote.
        /// </summary>
        [Required]
        [MinLength(1)]
        [MaxLength(Vote.PERSON_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// List of suitable dates for the person.
        /// </summary>
        [Required]
        [MinLength(1)]
        [DataType(DataType.Date)]
        public IEnumerable<DateTime> Votes { get; set; }
    }
}
