using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Eventsuffle.Core.Entities;
using Eventsuffle.FunctionalTests.ApiClient;
using Eventsuffle.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Eventsuffle.FunctionalTests
{
    [Collection("Run sequentially")]
    public abstract class BaseControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        protected BaseControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        protected EventsuffleApiClient GetApiClient()
        {
            var client = GetClient();
            return new EventsuffleApiClient(client.BaseAddress.ToString(), client);
        }

        protected BaseControllerTest(CustomWebApplicationFactory<Startup> factory) : this(factory as WebApplicationFactory<Startup>)
        {
        }

        protected async Task AddRangeToDbAsync<TDbContext, TEntity>(IEnumerable<TEntity> entities)
             where TDbContext : DbContext
             where TEntity : class
        {
            using (var scope = _factory.Services.CreateScope())
            {
                using (var db = (TDbContext)scope.ServiceProvider.GetService(typeof(TDbContext)))
                {
                    await db.Set<TEntity>().AddRangeAsync(entities).ConfigureAwait(false);
                    await db.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }

        private HttpClient GetClient()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
            _client = _factory.CreateClient();
            return _client;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    using (var scope = _factory.Services.CreateScope())
                    {
                        // Clean test database after each test.
                        using (var db = (EventSuffleDbContext)scope.ServiceProvider.GetService(typeof(EventSuffleDbContext)))
                        {
                            foreach (var c in db.Set<Event>())
                            {
                                db.Entry(c).State = EntityState.Deleted;
                            }
                            foreach (var c in db.Set<SuggestedDate>())
                            {
                                db.Entry(c).State = EntityState.Deleted;
                            }
                            foreach (var c in db.Set<Vote>())
                            {
                                db.Entry(c).State = EntityState.Deleted;
                            }
                            foreach (var c in db.Set<VoteSuggestedDate>())
                            {
                                db.Entry(c).State = EntityState.Deleted;
                            }
                            db.SaveChanges();
                        }
                    }
                    _client?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

}
