using System.Linq;
using RealEstate.Domain.Specifications;

namespace RealEstate.Infrastructure.Specifications
{
    public static class MongoLinqSpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> input, ISpecification<T> spec)
        {
            var query = input;

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);

            if (spec.Skip.HasValue) query = query.Skip(spec.Skip.Value);
            if (spec.Take.HasValue) query = query.Take(spec.Take.Value);

            return query;
        }

        public static IQueryable<TResult> GetQuery<T, TResult>(
            IQueryable<T> input, ISpecification<T, TResult> spec)
        {
            var baseQuery = GetQuery(input, (ISpecification<T>)spec);
            return baseQuery.Select(spec.Selector);
        }
    }
}
