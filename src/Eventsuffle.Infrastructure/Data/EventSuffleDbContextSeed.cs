using Eventsuffle.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eventsuffle.Infrastructure.Data
{
    public class EventSuffleDbContextSeed
    {
        private static readonly string[] _personNames = new string[] { "James", "Jill", "Jesse" };
        private static readonly string[] _eventNames = new string[] { "Barbeque", "Sauna", "Tennis" };
        private static readonly DateTime[] _suggestedDates = new DateTime[] { DateTime.Now.AddDays(10), DateTime.Now.AddDays(13), DateTime.Now.AddDays(14) };

        /// <summary>
        /// Seed development specific data.
        /// </summary>
        /// <param name="context">Database context.</param>
        public static Task SeedDevelopmentAsync(EventSuffleDbContext context)
        {
            foreach (var eventName in _eventNames)
            {
                var addedEvent = context.Events.Add(new Event { 
                    Name = eventName             
                });
                context.SaveChangesAsync();

                context.SuggestedDates
                    .AddRange(_suggestedDates
                    .Select(i => new SuggestedDate
                    {
                        Date = i,
                        EventId = addedEvent.Entity.Id,
                    }).ToArray());
                context.SaveChangesAsync();

                foreach (var personName in _personNames)
                {
                    var voteId = Guid.NewGuid();
                    context.Votes.Add(new Vote
                    {
                        Id = voteId,
                        PersonName = personName,
                        EventId = addedEvent.Entity.Id,
                        VoteSuggestedDates = addedEvent.Entity.SuggestedDates.Select(i => new VoteSuggestedDate
                        {
                            SuggestedDateId = i.Id,
                            VoteId = voteId,
                        }).ToArray()
                    });
                }
            }

            return context.SaveChangesAsync();
        }
    }
}
