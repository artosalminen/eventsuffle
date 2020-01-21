using Eventsuffle.Core.Entities;
using Eventsuffle.FunctionalTests.ApiClient;
using Eventsuffle.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Eventsuffle.FunctionalTests
{
    public class EventControllerTests: BaseControllerTest
    {
        public EventControllerTests(CustomWebApplicationFactory<Startup> factory): base(factory)
        {
        }
        
        [Fact]
        public async Task GetEventList_V1_Happy()
        {
            // Arrange
            var events = new Event[]
            {
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Event 1",
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Event 2",
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Event 3",
                }
            };

            await AddRangeToDbAsync<EventSuffleDbContext, Event>(events);

            var apiVersion = "1";
            var api = GetApiClient();

            // Act
            var result = await api.GetEventListAsync(apiVersion);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Events);
            Assert.Equal(3, result.Events.Count);
        }

        [Fact]
        public async Task GetEventWithId_V1_Happy()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var events = new Event[]
            {
                new Event
                {
                    Id = eventId,
                    Name = "Event 1",
                    SuggestedDates = new SuggestedDate[]
                    {
                        new SuggestedDate
                        {
                            Date = DateTime.Today.Date,
                        },
                        new SuggestedDate
                        {
                            Date = DateTime.Today.Date.AddDays(1),
                        },
                        new SuggestedDate
                        {
                            Date = DateTime.Today.Date.AddDays(2),
                        }
                    },
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Event 2",
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Event 3",
                }
            };

            await AddRangeToDbAsync<EventSuffleDbContext, Event>(events);


            var votes = new Vote[]
            {
                new Vote
                {
                    EventId = eventId,
                    PersonName = "Jake",
                    VoteSuggestedDates = new VoteSuggestedDate[]
                    {
                        new VoteSuggestedDate
                        {
                            SuggestedDateId = events.First().SuggestedDates.First().Id,
                        },
                        new VoteSuggestedDate
                        {
                            SuggestedDateId = events.First().SuggestedDates.Last().Id
                        }
                    }
                },
                new Vote
                {
                    EventId = eventId,
                    PersonName = "Elwood",
                    VoteSuggestedDates = new VoteSuggestedDate[]
                    {
                        new VoteSuggestedDate
                        {
                            SuggestedDateId = events.First().SuggestedDates.First().Id,
                        },
                        new VoteSuggestedDate
                        {
                            SuggestedDateId = events.First().SuggestedDates.Last().Id
                        }
                    }
                },
                new Vote
                {
                    EventId = eventId,
                    PersonName = "Penguin",
                    VoteSuggestedDates = new VoteSuggestedDate[]
                    {
                        new VoteSuggestedDate
                        {
                            SuggestedDateId = events.First().SuggestedDates.First().Id,
                        },
                    }
                }
            };

            await AddRangeToDbAsync<EventSuffleDbContext, Vote>(votes);

            var apiVersion = "1";
            var api = GetApiClient();

            // Act
            var result = await api.GetEventByIdAsync(eventId, apiVersion);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
            Assert.Equal(events.Single(i => i.Id == eventId).Name, result.Name);
            Assert.Equal(3, result.Dates.Count);
            Assert.Equal(2, result.Votes.Count);
        }

        [Fact]
        public async Task GetEventResultsWithId_V1_Happy()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var events = new Event[]
            {
                new Event
                {
                    Id = eventId,
                    Name = "Event 1",
                    SuggestedDates = new SuggestedDate[]
                    {
                        new SuggestedDate
                        {
                            Date = DateTime.Today.Date
                        },
                        new SuggestedDate
                        {
                            Date = DateTime.Today.Date.AddDays(1)
                        },
                        new SuggestedDate
                        {
                            Date = DateTime.Today.Date.AddDays(2)
                        }
                    },
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Event 2",
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Event 3",
                }
            };

            await AddRangeToDbAsync<EventSuffleDbContext, Event>(events);

            var votes = new Vote[]
            {
                new Vote
                {
                    EventId = eventId,
                    PersonName = "Jake",
                    VoteSuggestedDates = new VoteSuggestedDate[]
                    {
                        new VoteSuggestedDate
                        {
                            SuggestedDateId = events.First().SuggestedDates.First().Id,
                        },
                        new VoteSuggestedDate
                        {
                            SuggestedDateId = events.First().SuggestedDates.Last().Id
                        }
                    }
                },
                new Vote
                {
                    EventId = eventId,
                    PersonName = "Elwood",
                    VoteSuggestedDates = new VoteSuggestedDate[]
                    {
                        new VoteSuggestedDate
                        {
                            SuggestedDateId = events.First().SuggestedDates.First().Id,
                        },
                        new VoteSuggestedDate
                        {
                            SuggestedDateId = events.First().SuggestedDates.Last().Id
                        }
                    }
                },                
                new Vote
                {
                    EventId = eventId,
                    PersonName = "Penguin",
                    VoteSuggestedDates = new VoteSuggestedDate[]
                    {
                        new VoteSuggestedDate
                        {
                            SuggestedDateId = events.First().SuggestedDates.First().Id,
                        },
                    }
                }
            };

            await AddRangeToDbAsync<EventSuffleDbContext, Vote>(votes);

            var apiVersion = "1";
            var api = GetApiClient();

            // Act
            var result = await api.GetEventResultsByIdAsync(eventId, apiVersion);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
            Assert.Equal(events.Single(i => i.Id == eventId).Name, result.Name);
            Assert.Equal(1, result.SuitableDates.Count);
            foreach(var personName in votes.Select(i => i.PersonName))
            {
                Assert.True(result.SuitableDates.Single().People.Contains(personName));
            }
        }

        [Fact]
        public async Task PostEvent_V1_Happy()
        {
            // Arrange
            var eventToBeCreated = new EventCreateViewModel
            {
                Name = "Event name",
                Dates = new DateTime[]
                {
                    DateTime.Today
                }
            };

            var apiVersion = "1";
            var api = GetApiClient();

            // Act
            var result = await api.PostEventAsync(apiVersion, eventToBeCreated);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Fact]
        public async Task PostEventWithSuggestedDates_V1_Happy()
        {
            // Arrange
            var eventName = "Event name";
            var eventToBeCreated = new EventCreateViewModel
            {
                Name = eventName,
                Dates = new DateTime[]
                {
                    DateTime.Today,
                    DateTime.Today.AddDays(1),
                    DateTime.Today.AddDays(2),
                }
            };

            var apiVersion = "1";
            var api = GetApiClient();

            // Act
            var postResult = await api.PostEventAsync(apiVersion, eventToBeCreated);
            var result = await api.GetEventByIdAsync(postResult.Id, apiVersion);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(postResult.Id, result.Id);
            Assert.Equal(eventName, result.Name);
            Assert.Equal(3, result.Dates.Count);
            Assert.Equal(0, result.Votes.Count);
        }

        [Fact]
        public async Task PostVote_V1_Happy()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var events = new Event[]
            {
                new Event
                {
                    Id = eventId,
                    Name = "Event 1",
                    SuggestedDates = new SuggestedDate[]
                    {
                        new SuggestedDate
                        {
                            Date = DateTime.Today.Date,
                        },
                        new SuggestedDate
                        {
                            Date = DateTime.Today.Date.AddDays(1),
                        },
                        new SuggestedDate
                        {
                            Date = DateTime.Today.Date.AddDays(2),
                        }
                    },
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Event 2",
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Event 3",
                }
            };

            await AddRangeToDbAsync<EventSuffleDbContext, Event>(events);

            var datesToBeVoted = new DateTime[]
                {
                    DateTime.Today,
                    DateTime.Today.AddDays(1),
                };
            VoteCreateViewModel vote = new VoteCreateViewModel
            {
                Name = "Elwood",
                Votes = datesToBeVoted
            };

            var apiVersion = "1";
            var api = GetApiClient();

            // Act
            var result = await api.AddVoteToEventAsync(eventId, apiVersion, vote);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
            Assert.Equal(3, result.Dates.Count);
            Assert.Equal(2, result.Votes.Count);
            Assert.True(result.Votes.All(i => i.People.Single() == "Elwood"));
            foreach (var date in result.Votes.Select(i => i.Date))
            {
                Assert.Contains(date.Date, datesToBeVoted.ToList());
            }
        }
    }
}
