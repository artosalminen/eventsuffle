using Eventsuffle.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Eventsuffle.Core.Interfaces
{
    /// <summary>
    /// Wraps entity type with methods to access a underlying data store with read and write operations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to wrap.</typeparam>
    public interface IAsyncRepository<TEntity, in TKeyType> where TEntity : BaseEntity<TKeyType>
    {
        Task<TEntity> GetByIdAsync(TKeyType id);
        Task<TEntity> GetByIdAsync(
            TKeyType id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);
        Task<IReadOnlyList<TEntity>> ListAllAsync();
        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<int> CountAsync(ISpecification<TEntity> spec);
    }
}
