using Eventsuffle.Core.Entities;
using Eventsuffle.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventsuffle.Web.Extensions
{
    public static class ConvertEntityViewModelExtensions
    {
        public static ListEventViewModel ToListEventViewModel(this Event input)
        {
            return new ListEventViewModel()
            {
                Id = input.Id,
                Name = input.Name
            };
        }

        public static EventViewModel ToEventViewModel(this Event input)
        {
            var votedDates = input.Votes.SelectMany(v => v.VoteSuggestedDates);
            var voteVMs = votedDates
                .GroupBy(i => i.SuggestedDate.Date)
                .Select(g => g.First())
                .Select(i => new DateVotesViewModel
                {
                    Date = i.SuggestedDate.Date.Date,
                    People = votedDates.Where(vd => vd.SuggestedDateId == i.SuggestedDateId).Select(i => i.Vote.PersonName)
                });
            return new EventViewModel()
            {
                Id = input.Id,
                Name = input.Name,
                Dates = input.SuggestedDates?.Select(i => i.Date.Date).ToArray(),
                Votes = voteVMs,
            };
        }

        public static EventResultViewModel ToEventResultViewModel(this (Event theEvent, IDictionary<DateTime, List<string>> suitableDates) input)
        {
            return new EventResultViewModel
            {
                Id = input.theEvent.Id,
                Name = input.theEvent.Name,
                SuitableDates = input.suitableDates.Select(i => new DateVotesViewModel
                {
                    Date = i.Key,
                    People = i.Value
                })
            };
        }
    }
}
