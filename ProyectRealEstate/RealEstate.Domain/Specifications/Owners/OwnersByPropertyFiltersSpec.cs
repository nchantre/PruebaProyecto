using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Filter;
using RealEstate.Domain.Specifications;



public sealed class OwnersByPropertyFiltersSpec : BaseSpecification<Owner>
{
    public OwnersByPropertyFiltersSpec(PropertySearchParams p)
    {
        Criteria = o => o.Properties.Any(pr =>
            (string.IsNullOrEmpty(p.Name) || pr.Name.Contains(p.Name)) &&
            (string.IsNullOrEmpty(p.Address) || pr.Address.Contains(p.Address)) &&
            (!p.Price.HasValue || pr.Price == p.Price.Value) &&
            (string.IsNullOrEmpty(p.CodeInternal) || pr.CodeInternal == p.CodeInternal) &&
            (!p.Year.HasValue || pr.Year == p.Year.Value)
        );

        if (p.OrderByNameDesc) ApplyOrderByDescending(o => o.Name);
        else ApplyOrderBy(o => o.Name);

        var skip = (p.Page - 1) * p.PageSize;
        ApplyPaging(skip, p.PageSize);
    }
}
