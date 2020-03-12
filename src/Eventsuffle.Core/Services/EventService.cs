using Eventsuffle.Core.Entities;
using Eventsuffle.Core.Interfaces;
using Eventsuffle.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventsuffle.Core.Services
{
    public class EventService : IEventService
    {
        private readonly IAsyncRepository<Event, Guid> _eventRepository;
        private readonly IAsyncRepository<Vote, Guid> _voteRepository;
        private readonly IAsyncRepository<SuggestedDate, Guid> _suggestedDateRepository;
        private readonly IAsyncRepository<VoteSuggestedDate, Guid> _voteSuggestedDateRepository;

        public EventService(
            IAsyncRepository<Event, Guid> eventRepository,
            IAsyncRepository<Vote, Guid> voteRepository,
            IAsyncRepository<SuggestedDate, Guid> suggestedDateRepository,
            IAsyncRepository<VoteSuggestedDate, Guid> voteSuggestedDateRepository
            )
        {
            _eventRepository = eventRepository;
            _voteRepository = voteRepository;
            _suggestedDateRepository = suggestedDateRepository;
            _voteSuggestedDateRepository = voteSuggestedDateRepository;
        }

        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await _eventRepository.ListAllAsync();
        }

        public async Task<Event> GetEventByIdAsync(Guid eventId)
        {
            return await _eventRepository.GetByIdAsync(
                eventId,
                includes: e => e
                    .Include(i => i.SuggestedDates)
                    .Include(i => i.Votes)
                    .ThenInclude(i => i.VoteSuggestedDates)
                    .ThenInclude(i => i.SuggestedDate));
        }

        public async Task<Guid> CreateEventAsync(string eventName, IReadOnlyList<DateTime> dates)
        {
            var createdEvent = await _eventRepository.AddAsync(new Event
            {
                Name = eventName,
                SuggestedDates = dates.Select(i => new SuggestedDate
                {
                    Date = i,
                }).Distinct().ToArray()
            });
            return createdEvent.Id;
        }

        public async Task<Event> AddVotesToEventAsync(Guid eventId, string personName, IReadOnlyList<DateTime> votedDates)
        {
            // Check if there is already a vote for this event by the person
            var existingVote = (await _voteRepository.ListAsync(new VoteSpecifications(eventId, personName))).SingleOrDefault();

            var suggestedDateIds = (await _suggestedDateRepository.ListAsync(new SuggestedDateSpecifications(eventId, votedDates))).Select(i => i.Id);

            if (existingVote != null && suggestedDateIds.Any())
            {
                var existingVoteSuggestedDateIds = existingVote.VoteSuggestedDates.Select(i => i.SuggestedDateId).ToArray();
                var addedVotesForExistingDates = suggestedDateIds
                    .Where(suggestedDateId => !existingVoteSuggestedDateIds.Contains(suggestedDateId))
                    .Select(suggestedDateId =>
                    new VoteSuggestedDate
                    {
                        VoteId = existingVote.Id,
                        SuggestedDateId = suggestedDateId
                    }).ToArray();
                foreach(var addedVote in addedVotesForExistingDates)
                {
                    await _voteSuggestedDateRepository.AddAsync(addedVote);
                }
            } 
            else if (suggestedDateIds.Any())
            {
                await _voteRepository.AddAsync(new Vote
                {
                    PersonName = personName,
                    EventId = eventId,
                    VoteSuggestedDates = suggestedDateIds.Select(i => new VoteSuggestedDate
                    {
                        SuggestedDateId = i,
                    }).ToArray()
                });
            } else
            {
                return null;
            }

            return await GetEventByIdAsync(eventId);
        }

        public async Task<(Event theEvent, IDictionary<DateTime, List<string>> suitableDates)> GetEventResultsAsync(Guid eventId)
        {
            var theEvent = await _eventRepository.GetByIdAsync(
                eventId,
                includes: e => e
                    .Include(i => i.SuggestedDates)
                    .ThenInclude(i => i.VoteSuggestedDates)
                    .Include(i => i.Votes));
            if (theEvent == null) {
                return (theEvent, new Dictionary<DateTime, List<string>>());
            }

            var participantCount = theEvent.Votes.Count();
            var participants = theEvent.Votes.Select(i => i.PersonName);
            var suitableDates = theEvent.SuggestedDates
                .Where(i => i.VoteSuggestedDates.Count == participantCount)
                .ToDictionary(key => key.Date, value => participants.ToList());
            return (theEvent, suitableDates);
        }
    }
}
