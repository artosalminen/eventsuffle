using Eventsuffle.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsuffle.Core.Services
{
    public interface IEventService
    {
        /// <summary>
        /// Retrieve all persisted events.
        /// </summary>
        Task<IEnumerable<Event>> GetAllEvents();

        /// <summary>
        /// Retrieve identified event.
        /// </summary>
        /// <param name="eventId">Event identification.</param>
        /// <returns>Identified event with related entities.</returns>
        Task<Event> GetEventByIdAsync(Guid eventId);

        /// <summary>
        /// Create an event.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="dates">Suggested dates for the event.</param>
        /// <returns>Event identification.</returns>
        Task<Guid> CreateEventAsync(string eventName, IReadOnlyList<DateTime> dates);

        /// <summary>
        /// Adds votes to an event.
        /// </summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="personName">Name of the person adding a vote.</param>
        /// <param name="votedDates">Dates that are voted for.</param>
        /// <returns>Updated event with related entities.</returns>
        Task<Event> AddVotesToEventAsync(Guid eventId, string personName, IReadOnlyList<DateTime> votedDates);

        /// <summary>
        /// Retrieve identified event result.
        /// </summary>
        /// <param name="eventId">Event identification.</param>
        /// <returns>The event and dictionary of dates that are suitable for all participants and participant name list.</returns>
        Task<(Event theEvent, IDictionary<DateTime, List<string>> suitableDates)> GetEventResultsAsync(Guid eventId);
    }
}