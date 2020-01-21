using System;
using System.Linq;
using System.Threading.Tasks;
using Eventsuffle.Core.Services;
using Eventsuffle.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Eventsuffle.Web.Extensions;
using Microsoft.AspNetCore.Http;

namespace Eventsuffle.Web.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(
            IEventService eventService
            )
        {
            _eventService = eventService;
        }

        [HttpGet("list", Name = nameof(GetEventListAsync))]
        [ProducesResponseType(typeof(ListOfEventsViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventListAsync()
        {
            var events = (await _eventService.GetAllEvents())
                .Select(i => i.ToListEventViewModel());
            return Ok(new ListOfEventsViewModel(events));
        }

        [HttpGet("{id}", Name = nameof(GetEventByIdAsync))]
        [ProducesResponseType(typeof(EventViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventByIdAsync([FromRoute]Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var foundEvent = await _eventService.GetEventByIdAsync(id);
            if (foundEvent == null)
            {
                return NotFound();
            }
            return Ok(foundEvent.ToEventViewModel());
        }

        [HttpPost(Name = nameof(PostEventAsync))]
        [ProducesResponseType(typeof(BaseViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostEventAsync([FromBody]EventCreateViewModel eventToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdEventId = await _eventService.CreateEventAsync(eventToCreate.Name, eventToCreate.Dates.ToList());
            return Created(nameof(GetEventByIdAsync), new BaseViewModel { Id = createdEventId });
        }

        [HttpPost("{eventId}/vote", Name = nameof(AddVoteToEventAsync))]
        [ProducesResponseType(typeof(EventViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddVoteToEventAsync([FromRoute]Guid eventId, [FromBody]VoteCreateViewModel voteToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedEvent = await _eventService.AddVotesToEventAsync(eventId, voteToCreate.Name, voteToCreate.Votes.ToList());
            return Ok(updatedEvent.ToEventViewModel());
        }

        [HttpGet("{id}/results", Name = nameof(GetEventResultsByIdAsync))]
        [ProducesResponseType(typeof(EventResultViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventResultsByIdAsync([FromRoute]Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _eventService.GetEventResultsAsync(id);
            if (result.theEvent == null)
            {
                return NotFound();
            }
            return Ok(result.ToEventResultViewModel());
        }
    }
}
