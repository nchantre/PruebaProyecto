using System;
using System.Linq.Expressions;

namespace RealEstate.Domain.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification() { }
        protected BaseSpecification(Expression<Func<T, bool>> criteria) => Criteria = criteria;

        public Expression<Func<T, bool>>? Criteria { get; protected set; }
        public Expression<Func<T, object>>? OrderBy { get; protected set; }
        public Expression<Func<T, object>>? OrderByDescending { get; protected set; }
        public int? Skip { get; protected set; }
        public int? Take { get; protected set; }

        protected void ApplyOrderBy(Expression<Func<T, object>> orderBy) => OrderBy = orderBy;
        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDesc) => OrderByDescending = orderByDesc;
        protected void ApplyPaging(int skip, int take) { Skip = skip; Take = take; }
    }

    public abstract class BaseSpecification<T, TResult> : BaseSpecification<T>, ISpecification<T, TResult>
    {
        protected BaseSpecification() { }
        protected BaseSpecification(Expression<Func<T, bool>> criteria) : base(criteria) { }

        public Expression<Func<T, TResult>> Selector { get; protected set; } = default!;
        protected void ApplySelector(Expression<Func<T, TResult>> selector) => Selector = selector;
    }
}
