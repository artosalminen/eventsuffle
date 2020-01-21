using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Eventsuffle.Core.Interfaces
{
    /// <summary>
    /// Search specification for entities.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity for which the specification is created for.</typeparam>
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }
}
