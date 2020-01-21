using Eventsuffle.Core.Entities;
using Eventsuffle.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Eventsuffle.Infrastructure.Data
{
    public class EfSpecificationEvaluator<TEntity, TKeyType> where TEntity : BaseEntity<TKeyType>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var query = inputQuery;

            // modify the IQueryable using the specification's criteria expression
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Includes all expression-based includes
            query = specification.Includes.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Include any string-based include statements
            query = specification.IncludeStrings.Aggregate(query,
                                    (current, include) => current.Include(include));

            return query;
        }
    }
}
