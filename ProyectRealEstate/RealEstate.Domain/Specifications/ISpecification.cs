using System.Linq.Expressions;

namespace RealEstate.Domain.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }
        int? Skip { get; }
        int? Take { get; }
    }

    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>> Selector { get; }
    }
}
